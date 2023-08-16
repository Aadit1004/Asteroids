using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;
    [SerializeField] private bool isBigAsteroid = false;
    private int lives;

    void Start()
    {
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        lives = (isBigAsteroid) ? 3 : 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary") 
        {
            asteroidManager.removeAsteroid(this.gameObject);
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
