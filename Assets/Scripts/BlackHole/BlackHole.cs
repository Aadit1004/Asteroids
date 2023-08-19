using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    [SerializeField] private GameObject spaceShip;

    [SerializeField] private GameObject gameManagerObject;
    private GameScript gameManager;

    [SerializeField] private GameObject spaceBombManagerObj;
    private SpaceBombManager spaceBombManager;

    public ParticleSystem spaceshipExplosion;

    [SerializeField] private GameObject blackHoleManagerObj;
    private BlackHoleManager blackHoleManager;

    bool isShipDead = false;
    bool cooldown = false;

    [SerializeField] private GameObject smallAstSoundObj;
    private AudioSource smallAstSoundEffect;

    [SerializeField] private GameObject largeAstSoundObj;
    private AudioSource largeAstSoundEffect;

    public ParticleSystem redExplosion;
    [SerializeField] private GameObject powerUpManagerObj;
    private PowerUpsManager powerUpsManager;

    [SerializeField] private GameObject powerUpSoundObj;
    private AudioSource powerUpSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        gameManager = gameManagerObject.GetComponent<GameScript>();
        spaceBombManager = spaceBombManagerObj.GetComponent<SpaceBombManager>();
        blackHoleManager = blackHoleManagerObj.GetComponent<BlackHoleManager>();
        largeAstSoundEffect = largeAstSoundObj.GetComponent<AudioSource>();
        smallAstSoundEffect = smallAstSoundObj.GetComponent<AudioSource>();
        powerUpsManager = powerUpManagerObj.GetComponent<PowerUpsManager>();
        powerUpSoundEffect = powerUpSoundObj.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive())
        {

            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(this.transform.position, 5f); // Query all objects within radius of 5 units

            foreach (Collider2D col in affectedObjects)
            {
                if (col.gameObject.tag == "Player") // if ship
                {
                    float distance = Vector2.Distance(this.transform.position, spaceShip.transform.position);
                    if (distance <= 5f && gameManager.canCollideWithThreats())
                    {
                        Vector2 direction = (this.transform.position - spaceShip.transform.position).normalized; //  direction from the spaceship to the black hole
                        float forceMagnitude = (5 - distance) * 0.95f; // force magnitude increases as ship gets closer, linear increase
                        Vector2 gravitationalForce = direction * forceMagnitude; // vector of gravity direction
                        spaceShip.GetComponent<Rigidbody2D>().AddForce(gravitationalForce);
                        if (distance < 0.5f && !isShipDead)
                        {
                            isShipDead = true;
                            gameManager.suckedInBlackHolde();
                            spaceshipExplosion.transform.position = spaceShip.transform.position;
                            spaceshipExplosion.Play();
                            spaceShip.GetComponent<SpaceShip>().resetPowerUp();
                            gameManager.respawnShip();
                        }
                    }
                }
                if (col.gameObject.tag == "AsteroidBig" || col.gameObject.tag == "AsteroidSmall" || col.gameObject.tag == "SpaceBomb")
                {
                    GameObject other = col.gameObject;
                    float distance = Vector2.Distance(this.transform.position, other.transform.position);
                    if (distance <= 5f )
                    {
                        Vector2 direction = (this.transform.position - other.transform.position).normalized;
                        float forceMagnitude = (5 - distance) * 1.05f; 
                        Vector2 gravitationalForce = direction * forceMagnitude;
                        other.GetComponent<Rigidbody2D>().AddForce(gravitationalForce);
                        if (distance < 1f && !cooldown && (other.tag == "AsteroidSmall" || other.tag == "SpaceBomb" || other.tag == "AsteroidBig"))
                        {
                            cooldown = true;
                            if (other.tag == "AsteroidSmall")
                            {
                                smallAstSoundEffect.Play();
                                asteroidManager.removeAsteroid(other.gameObject);
                            }
                            if (other.tag == "AsteroidBig")
                            {
                                largeAstSoundEffect.Play();
                                asteroidManager.removeAsteroid(other.gameObject);
                            } 
                            else if (other.tag == "SpaceBomb")
                            {
                                largeAstSoundEffect.Play();
                                spaceBombManager.removeBomb(other.gameObject);
                            }
                            spaceshipExplosion.transform.position = other.transform.position;
                            Destroy(other);
                            spaceshipExplosion.Play();
                            StartCoroutine(delayCooldown());
                        }
                    }
                }
                if (col.gameObject.tag == "TimeDilation" || col.gameObject.tag == "Shield" || col.gameObject.tag == "BurstFire")
                {
                    GameObject other = col.gameObject;
                    float distance = Vector2.Distance(this.transform.position, other.transform.position);
                    if (distance <= 5f)
                    {
                        Vector2 direction = (this.transform.position - other.transform.position).normalized;
                        float forceMagnitude = (5 - distance) * 1.05f;
                        Vector2 gravitationalForce = direction * forceMagnitude;
                        other.GetComponent<Rigidbody2D>().AddForce(gravitationalForce);
                        if (distance < 1f && !cooldown && (other.tag == "TimeDilation" || other.tag == "Shield" || other.tag == "BurstFire"))
                        {
                            cooldown = true;
                            powerUpSoundEffect.Play();
                            powerUpsManager.removePowerUp(other.gameObject);
                            redExplosion.transform.position = other.transform.position;
                            Destroy(other);
                            redExplosion.Play();
                            StartCoroutine(delayCooldown());
                        }
                    }
                }
            }
        }
    }

    IEnumerator delayCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }

    public void resetValueForBlackHole() { isShipDead = false; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary")
        {
            asteroidManager.resetMaxAsteroids();

            blackHoleManager.removeBlackHole(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
