using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject gameManager;
    private GameScript gameScript;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    public ParticleSystem explosion;

    [SerializeField] private GameObject bombManagerObj;
    private SpaceBombManager spaceBombManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameScript = gameManager.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    GameObject other = collision.gameObject;

    //    if (other.tag == "Boundary")
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    if (other.tag == "AsteroidBig")
    //    {
    //        Asteroid asteroidScript = other.GetComponent<Asteroid>();
    //        asteroidScript.detuctLife();
    //        if (asteroidScript.getLives() == 0)
    //        {
    //            gameScript.hitBigAsteroid();
    //            asteroidManager.removeAsteroid(other);
    //            Destroy(other.gameObject);
    //        }
    //        Destroy(this.gameObject);
    //    }
    //    if (other.tag == "AsteroidSmall")
    //    {
    //        Asteroid asteroidScript = other.GetComponent<Asteroid>();
    //        asteroidScript.detuctLife();
    //        if (asteroidScript.getLives() == 0)
    //        {
    //            gameScript.hitSmallAsteroid();
    //            asteroidManager.removeAsteroid(other);
    //            Destroy(other.gameObject);
    //        }
    //        Destroy(this.gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "Boundary")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "AsteroidBig" || other.tag == "AsteroidSmall")
        {
            Asteroid asteroidScript = other.GetComponent<Asteroid>();
            asteroidScript.detuctLife();
            if (asteroidScript.getLives() == 0)
            {
                // play asteroid explosion sound
                if (other.tag == "AsteroidBig")
                    gameScript.hitBigAsteroid();
                else
                    gameScript.hitSmallAsteroid();

                asteroidManager.removeAsteroid(other);
                Destroy(other.gameObject);
                this.explosion.transform.position = this.transform.position;
                this.explosion.Play();
            } 
            else
            {
                // play asteroid hit sound
            }
            Destroy(this.gameObject);
        }
        if (other.tag == "SpaceBomb")
        {
            SpaceBomb bombScript = other.GetComponent<SpaceBomb>();
            bombScript.detuctLife();
            if (bombScript.getLives() == 0)
            {
                // play asteroid explosion sound
                gameScript.hitSpaceBomb();
                spaceBombManager.removeBomb(other);
                Destroy(other.gameObject);
                this.explosion.transform.position = this.transform.position;
                this.explosion.Play();
            }
            Destroy(this.gameObject);
        }
    }
}
