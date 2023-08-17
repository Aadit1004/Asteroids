using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    [SerializeField] private GameObject spaceShip;

    [SerializeField] private GameObject gameManagerObject;
    private GameScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        gameManager = gameManagerObject.GetComponent<GameScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive())
        {
            float distance = Vector2.Distance(this.transform.position, spaceShip.transform.position);
            if (distance <= 5f)
            {
                Vector2 direction = (this.transform.position - spaceShip.transform.position).normalized; //  direction from the spaceship to the black hole
                float forceMagnitude = (5 - distance) * 0.75f; // force magnitude increases as ship gets closer, linear increase
                Vector2 gravitationalForce = direction * forceMagnitude; // vector of gravity direction
                spaceShip.GetComponent<Rigidbody2D>().AddForce(gravitationalForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary")
        {
            asteroidManager.removeAsteroid(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        asteroidManager.resetMaxAsteroids();
    }
}
