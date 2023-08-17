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
            if (Vector2.Distance(this.transform.position, spaceShip.transform.position) <= 5)
            {
                Debug.Log("sucking player in black hole");
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
