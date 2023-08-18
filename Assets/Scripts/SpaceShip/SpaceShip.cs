using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

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
    private float rotationAmount = 30f;
    private float thrustAmount = 1.3f;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        gameManager = gameManagerObject.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
        currentPowerUp = PowerUp.None;
    }

    void Update()
    {
        //if (gameManager.isGameActive())
        displayText();
        if (true)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    // If the current angular velocity exceeds the maximum, set the angular velocity to the maximum, preserving the sign
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                } else
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
                    // start coroutine
                    // reset powerup to none and isPowerUpActive = false
                    StartCoroutine(burstFireDelay());
                }
                else
                {
                    Vector2 spawnPosition = transform.position + transform.up * spawnDistance;
                    GameObject newMissile = Instantiate(missile, spawnPosition, Quaternion.identity);
                    newMissile.GetComponent<Rigidbody2D>().velocity = GetFacingDirection() * missileSpeed;
                }
                
            } // fire missile
            if (Input.GetKey(KeyCode.E) && currentPowerUp != PowerUp.None) 
            {
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

    #region Delete Later

    public TMP_Text boolVal;
    public TMP_Text currentVal;

    private void displayText()
    {
        boolVal.text = "isPowerUpActive: " + isPowerUpActive;
        currentVal.text = "Current PowerUp: " + currentPowerUp;
    }

    #endregion

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "TimeDilation")
        {
            if (currentPowerUp == PowerUp.None)
            {
                currentPowerUp = PowerUp.TimeDilation;
                Destroy(other.gameObject);
            }
                
        }
        if (other.tag == "Shield")
        {
            if (currentPowerUp == PowerUp.None)
            {
                currentPowerUp = PowerUp.Shield;
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "BurstFire")
        {
            if (currentPowerUp == PowerUp.None)
            {
                currentPowerUp = PowerUp.BurstFire;
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
        // stop powerup coroutines and timers
    }

    public bool shieldIsActive() { return currentPowerUp == PowerUp.Shield && isPowerUpActive; }
    public bool timeDilationIsActive() { return currentPowerUp == PowerUp.TimeDilation && isPowerUpActive; }
}