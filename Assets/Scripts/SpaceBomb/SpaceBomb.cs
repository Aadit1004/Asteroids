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

    void Start()
    {
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
        gameManager = gameManagerObject.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
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
        if ((Vector2.Distance(this.transform.position, spaceship.transform.position) <= 2.5f) && gameManager.canCollideWithThreats())
        {
            gameManager.hitBomb();
            spaceshipExplosion.transform.position = spaceship.transform.position;
            spaceshipExplosion.Play();
            gameManager.respawnShip();
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
        asteroidManager.resetMaxAsteroids();
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireSphere(transform.position, 2.5f);
    //}
}
