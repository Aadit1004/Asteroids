using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBomb : MonoBehaviour
{
    private int lives = 2;

    [SerializeField] private GameObject bombManagerObj;
    private SpaceBombManager spaceBombManager;

    public ParticleSystem[] bombExplosions;


    void Start()
    {
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
    }

    public void startExplosion()
    {
        Debug.Log("Starting Explosion");
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
        spaceBombManager.removeBomb(this.gameObject);
        Destroy(this.gameObject);
        Debug.Log("Explosion Completed");
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
}
