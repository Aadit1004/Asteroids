using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBomb : MonoBehaviour
{
    private int lives = 2;

    [SerializeField] private GameObject bombManagerObj;
    private SpaceBombManager spaceBombManager;

    public ParticleSystem[] bombExplosions;

    [SerializeField] private GameObject gameManagerObject;

    private GameScript gameManager;

    [SerializeField] private GameObject spaceship;

    public ParticleSystem spaceshipExplosion;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;


    [SerializeField] private GameObject blackHoleManagerObj;
    private BlackHoleManager blackHoleManager;

    [SerializeField] private GameObject spaceBombExplosionSoundObj;
    private AudioSource spacebombExplosionSoundEffect;

    void Start()
    {
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
        gameManager = gameManagerObject.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        blackHoleManager = blackHoleManagerObj.GetComponent<BlackHoleManager>();
        spacebombExplosionSoundEffect = spaceBombExplosionSoundObj.GetComponent<AudioSource>(); 
    }

    public void startExplosion()
    {
        StartCoroutine(explosionDelay());
    }

    private IEnumerator explosionDelay()
    {
        yield return new WaitForSeconds(Random.Range(5f, 7f));
        for (int i = 0; i < bombExplosions.Length; i++)
        {
            ParticleSystem explosion = bombExplosions[i];
            explosion.transform.position = transform.position;
            explosion.Play();
        }
        spacebombExplosionSoundEffect.Play();
        if ((Vector2.Distance(this.transform.position, spaceship.transform.position) <= 3f) && gameManager.canCollideWithThreats())
        {
            if (!(spaceship.GetComponent<SpaceShip>().shieldIsActive()))
            {
                spaceship.GetComponent<SpaceShip>().resetPowerUp();
                gameManager.hitBomb();
                spaceshipExplosion.transform.position = spaceship.transform.position;
                spaceshipExplosion.Play();
                gameManager.respawnShip();
            }
        }
        spaceBombManager.removeBomb(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary")
        {
            spaceBombManager.removeBomb(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public int getLives()
    {
        return lives;
    }

    public void detuctLife()
    {
        lives--;
    }

    private void OnDestroy()
    {
        if (blackHoleManager.getNumBlackHoles() == 0 && spaceBombManager.getNumBombs() == 0) asteroidManager.resetMaxAsteroids();
    }
}
