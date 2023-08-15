using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    void Start()
    {
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary") 
        {
            asteroidManager.removeAsteroid();
            Destroy(this.gameObject);
        }
    }
}
