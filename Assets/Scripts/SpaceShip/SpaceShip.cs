using System.Collections;
using TMPro;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum PowerUp
{
    None,
    BurstFire,
    Shield,
    TimeDilation
}

public class SpaceShip : MonoBehaviour
{
    private Rigidbody2D rb;
    private float rotationAmount = 75f; // 30f old val
    private float thrustAmount = 5f; //1.3f old val
    private const float maxThrust = 5;
    private float maxAngularVelocity = 200f;

    [SerializeField] private GameObject missile;
    private float missileSpeed = 8f;
    private float spawnDistance = 0.8f;

    [SerializeField] private GameObject gameManagerObject;
    private GameScript gameManager;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    [SerializeField] private GameObject bombManagerObj;
    private SpaceBombManager spaceBombManager;

    // PowerUps

    PowerUp currentPowerUp = PowerUp.None;
    private bool isPowerUpActive = false;
    private float powerUpDuration = 0.0f;
    Coroutine powerUpCoroutineTimer;
    bool isCoroutineRunning = false;

    [SerializeField] private GameObject dullHitObj;
    private AudioSource dullHitSoundEffect;

    [SerializeField] private GameObject powerUpSoundObj;
    private AudioSource powerUpSoundEffect;

    [SerializeField] private GameObject missileShotSoundObj;
    private AudioSource missileShotSoundEffect;

    [SerializeField] private GameObject powerUpManagerObj;
    private PowerUpsManager powerUpsManager;

    public ParticleSystem blueExplosion;
    public ParticleSystem redExplosion;

    public TMP_Text powerUpText;
    public TMP_Text powerUpStatusText;

    [SerializeField] private GameObject powerUpStartObj;
    private AudioSource powerUpStartSoundEffect;
    private bool playedAlready = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        gameManager = gameManagerObject.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
        powerUpsManager = powerUpManagerObj.GetComponent<PowerUpsManager>();
        currentPowerUp = PowerUp.None;
        dullHitSoundEffect = dullHitObj.GetComponent<AudioSource>();
        powerUpSoundEffect = powerUpSoundObj.GetComponent<AudioSource>();
        missileShotSoundEffect = missileShotSoundObj.GetComponent<AudioSource>();
        powerUpStartSoundEffect = powerUpStartObj.GetComponent<AudioSource>();
    }

    void Update()
    {
        displayText();
        if (gameManager.isGameActive())
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    // If the current angular velocity exceeds the maximum, set the angular velocity to the maximum, preserving the sign
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                }
                else
                {
                    rb.AddTorque(-rotationAmount * Time.fixedDeltaTime);
                }
            } // rotate right
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                }
                else
                {
                    rb.AddTorque(rotationAmount * Time.fixedDeltaTime);
                }
            } // rotate left
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (rb.velocity.magnitude > maxThrust)
                {
                    rb.velocity = rb.velocity.normalized * maxThrust;
                }
                else
                {
                    rb.AddForce(GetFacingDirection() * thrustAmount);
                }
            } // add thrust
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (currentPowerUp == PowerUp.BurstFire && isPowerUpActive)
                {
                    StartCoroutine(burstFireDelay());
                }
                else
                {
                    missileShotSoundEffect.Play();
                    Vector2 spawnPosition = transform.position + transform.up * spawnDistance;
                    GameObject newMissile = Instantiate(missile, spawnPosition, Quaternion.identity);
                    newMissile.GetComponent<Rigidbody2D>().velocity = GetFacingDirection() * missileSpeed;
                }

            } // fire missile
            if (Input.GetKey(KeyCode.E) && currentPowerUp != PowerUp.None) 
            {
                if (!playedAlready)
                {
                    powerUpStartSoundEffect.Play();
                    playedAlready = true;
                }
                powerUpStatusText.text = "Active";
                powerUpStatusText.color = Color.green;
                isPowerUpActive = true;
                switch (currentPowerUp)
                {
                    case PowerUp.Shield:
                        powerUpDuration = 10f;
                        break;
                    case PowerUp.BurstFire:
                        powerUpDuration = 8f;
                        break;
                    case PowerUp.TimeDilation:
                        powerUpDuration = 6f;
                        activateTimeDilation();
                        break;
                }
                powerUpCoroutineTimer = StartCoroutine(DurationOfPowerUp(powerUpDuration));
            }
        }
    }

    //private void FixedUpdate()
    //{
    //    if (gameManager.isGameActive())
    //    {
    //        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    //        {
    //            if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
    //            {
    //                // If the current angular velocity exceeds the maximum, set the angular velocity to the maximum, preserving the sign
    //                rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
    //            }
    //            else
    //            {
    //                rb.AddTorque(-rotationAmount * Time.fixedDeltaTime);
    //            }
    //        } // rotate right
    //        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    //        {
    //            if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
    //            {
    //                rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
    //            }
    //            else
    //            {
    //                rb.AddTorque(rotationAmount * Time.fixedDeltaTime);
    //            }
    //        } // rotate left
    //        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    //        {
    //            if (rb.velocity.magnitude > maxThrust)
    //            {
    //                rb.velocity = rb.velocity.normalized * maxThrust;
    //            }
    //            else
    //            {
    //                rb.AddForce(GetFacingDirection() * thrustAmount);
    //            }
    //        } // add thrust
    //        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
    //        {
    //            if (currentPowerUp == PowerUp.BurstFire && isPowerUpActive)
    //            {
    //                StartCoroutine(burstFireDelay());
    //            }
    //            else
    //            {
    //                missileShotSoundEffect.Play();
    //                Vector2 spawnPosition = transform.position + transform.up * spawnDistance;
    //                GameObject newMissile = Instantiate(missile, spawnPosition, Quaternion.identity);
    //                newMissile.GetComponent<Rigidbody2D>().velocity = GetFacingDirection() * missileSpeed;
    //            }

    //        } // fire missile
    //    }
    //}

    private void displayText()
    {
        string currPowerUp = string.Empty;
        switch (currentPowerUp)
        {
            case PowerUp.None:
                currPowerUp = "None";
                break;
            case PowerUp.Shield:
                currPowerUp = "Shield";
                break;
            case PowerUp.BurstFire:
                currPowerUp = "Burst Fire";
                break;
            case PowerUp.TimeDilation:
                currPowerUp = "Time Dilation";
                break;
        }
        powerUpText.text = "Current Power-Up Equipped: " + currPowerUp;
    }

    private void activateTimeDilation()
    {
        asteroidManager.setAsteroidsSpeed();
        spaceBombManager.setsSpaceBombSpeed();
    }

    IEnumerator burstFireDelay()
    {
        int count = 3;
        while (count > 0)
        {
            missileShotSoundEffect.Play();
            Vector2 spawnPosition = transform.position + transform.up * spawnDistance;
            GameObject newMissile = Instantiate(missile, spawnPosition, Quaternion.identity);
            newMissile.GetComponent<Rigidbody2D>().velocity = GetFacingDirection() * missileSpeed;
            count--;
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    IEnumerator DurationOfPowerUp(float durationTime)
    {
        isCoroutineRunning = true;
        yield return new WaitForSecondsRealtime(durationTime);
        if (currentPowerUp == PowerUp.TimeDilation)
        {
            asteroidManager.resetAsteroidsSpeed();
            spaceBombManager.resetSpaceBombSpeed();
        }
        currentPowerUp = PowerUp.None;
        isPowerUpActive = false;
        powerUpDuration = 0.0f;
        isCoroutineRunning = false;
        powerUpStatusText.text = "Not Active";
        powerUpStatusText.color = Color.red;
        playedAlready = false;
    }

    private Vector2 GetFacingDirection()
    {
        float radians = (90 + transform.eulerAngles.z) * Mathf.Deg2Rad; // Convert rotation to radians
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // Return unit vector
    }

    public ParticleSystem[] bombExplosions;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Boundary")
        {
            //Debug.Log("Collided with boundary");
        }
        if ((other.tag == "AsteroidBig" || other.tag == "AsteroidSmall") && gameManager.canCollideWithThreats())
        {
            if (!(currentPowerUp == PowerUp.Shield && isPowerUpActive))
            {
                resetPowerUp();
                gameManager.hitAsteroid();
                asteroidManager.removeAsteroid(other);
                Destroy(other.gameObject);
                gameManager.respawnShip();
            }
        }
        if (other.tag == "SpaceBomb")
        {
            if (!(currentPowerUp == PowerUp.Shield && isPowerUpActive))
            {
                resetPowerUp();
                gameManager.hitBomb();
                spaceBombManager.removeBomb(other);
                for (int i = 0; i < bombExplosions.Length; i++)
                {
                    ParticleSystem explosion = bombExplosions[i];
                    explosion.transform.position = other.transform.position;
                    explosion.Play();
                }
                Destroy(other.gameObject);
                gameManager.respawnShip();
            }
        }


        // prev triggerenter2d code

        //checkPowerUp(collision.gameObject); // uncomment if want spaceship to pickup powerups if clashing into each other
        if (other.tag == "TimeDilation" || other.tag == "Shield" || other.tag == "BurstFire")
        {
            dullHitSoundEffect.Play();
            powerUpsManager.removePowerUp(other);
            redExplosion.transform.position = other.transform.position;
            redExplosion.Play();
            Destroy(other.gameObject);
        }
    }



    public void checkPowerUp(GameObject other)
    {
        if (other.tag == "TimeDilation")
        {
            if (currentPowerUp == PowerUp.None)
            {
                powerUpSoundEffect.Play();
                currentPowerUp = PowerUp.TimeDilation;
                powerUpsManager.removePowerUp(other);
                blueExplosion.transform.position = other.transform.position;
                blueExplosion.Play();
                Destroy(other.gameObject);
            } 
            else
            {
                dullHitSoundEffect.Play();
                powerUpsManager.removePowerUp(other);
                redExplosion.transform.position = other.transform.position;
                redExplosion.Play();
                Destroy(other.gameObject);
            }

        }
        if (other.tag == "Shield")
        {
            if (currentPowerUp == PowerUp.None)
            {
                powerUpSoundEffect.Play();
                currentPowerUp = PowerUp.Shield;
                powerUpsManager.removePowerUp(other);
                blueExplosion.transform.position = other.transform.position;
                blueExplosion.Play();
                Destroy(other.gameObject);
            }
            else
            {
                dullHitSoundEffect.Play();
                powerUpsManager.removePowerUp(other);
                redExplosion.transform.position = other.transform.position;
                redExplosion.Play();
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "BurstFire")
        {
            if (currentPowerUp == PowerUp.None)
            {
                powerUpSoundEffect.Play();
                currentPowerUp = PowerUp.BurstFire;
                powerUpsManager.removePowerUp(other);
                blueExplosion.transform.position = other.transform.position;
                blueExplosion.Play();
                Destroy(other.gameObject);
            }
            else
            {
                dullHitSoundEffect.Play();
                powerUpsManager.removePowerUp(other);
                redExplosion.transform.position = other.transform.position;
                redExplosion.Play();
                Destroy(other.gameObject);
            }
        }
    }

    public void resetPowerUp()
    {
        if (isCoroutineRunning) StopCoroutine(powerUpCoroutineTimer);
        if (currentPowerUp == PowerUp.TimeDilation)
        {
            asteroidManager.resetAsteroidsSpeed();
            spaceBombManager.resetSpaceBombSpeed();
        }
        currentPowerUp = PowerUp.None;
        isPowerUpActive = false;
        powerUpDuration = 0.0f;
        powerUpStatusText.text = "Not Active";
        powerUpStatusText.color = Color.red;
        playedAlready = false;
    }

    public bool shieldIsActive() { return currentPowerUp == PowerUp.Shield && isPowerUpActive; }
    public bool timeDilationIsActive() { return currentPowerUp == PowerUp.TimeDilation && isPowerUpActive; }
}