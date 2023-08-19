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

    [SerializeField] private GameObject dullHitObj;
    private AudioSource dullHitSoundEffect;

    [SerializeField] private GameObject smallAstSoundObj;
    private AudioSource smallAstSoundEffect;

    [SerializeField] private GameObject largeAstSoundObj;
    private AudioSource largeAstSoundEffect;

    // Start is called before the first frame update
    void Awake()
    {
        gameScript = gameManager.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
        dullHitSoundEffect = dullHitObj.GetComponent<AudioSource>();
        largeAstSoundEffect = largeAstSoundObj.GetComponent<AudioSource>();
        smallAstSoundEffect = smallAstSoundObj.GetComponent<AudioSource>();
    }

    [SerializeField] private GameObject spaceShip;

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
                if (other.tag == "AsteroidBig")
                {
                    largeAstSoundEffect.Play();
                    gameScript.hitBigAsteroid();
                }
                else
                {
                    smallAstSoundEffect.Play();
                    gameScript.hitSmallAsteroid();
                }
                    

                asteroidManager.removeAsteroid(other);
                Destroy(other.gameObject);
                this.explosion.transform.position = this.transform.position;
                this.explosion.Play();
            }
            else
            {
                // add sound effect?
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
                largeAstSoundEffect.Play();
                gameScript.hitSpaceBomb();
                spaceBombManager.removeBomb(other);
                Destroy(other.gameObject);
                this.explosion.transform.position = this.transform.position;
                this.explosion.Play();
            }
            else
            {
                // add sound effect?
            }
            Destroy(this.gameObject);
        }
        if (other.tag == "BlackHole")
        {
            float distance = Vector2.Distance(other.transform.position, this.transform.position);
            if (distance < 1) Destroy(this.gameObject);
            
        }
        if (other.tag == "Shield" || other.tag == "TimeDilation" || other.tag == "BurstFire")
        {
            spaceShip.gameObject.GetComponent<SpaceShip>().checkPowerUp(other.gameObject);
        }
    }
}
