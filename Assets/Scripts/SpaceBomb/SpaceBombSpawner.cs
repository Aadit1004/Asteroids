using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBombSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spaceBomb;
    [SerializeField] private GameObject spaceBombManagerObj;
    private SpaceBombManager spaceBombManager;
    [SerializeField] private GameObject spaceShip;

    // degrees (as unit circle {0/360 to right, 180 to left})
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;

    public bool leftSpawner = false;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    void Start()
    {
        spaceBombManager = spaceBombManagerObj.GetComponent<SpaceBombManager>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
    }

    public void startSpawner()
    {
        StartCoroutine(shootBomb());
    }

    private IEnumerator shootBomb()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20f, 25f)); // wait time - (20f, 25f) default
            asteroidManager.setMaxAsteroids(8);
            yield return new WaitForSeconds(3);
            if (spaceBombManager.getNumBombs() < spaceBombManager.getMaxBombs())
            {
                Vector3 spawnPos = transform.position;
                GameObject newBomb = Instantiate(spaceBomb, spawnPos, Quaternion.identity);
               
                Vector2 direction = getDirection();
                float bombSpeed = 2f;
                newBomb.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * bombSpeed;
                float torqueMagnitude = Random.Range(10f, 20f);
                float torqueDirection = Random.Range(0, 2) == 0 ? 1 : -1;
                float finalTorque = torqueMagnitude * torqueDirection;
                newBomb.GetComponent<Rigidbody2D>().AddTorque(finalTorque);
                if (spaceShip.gameObject.GetComponent<SpaceShip>().timeDilationIsActive())
                {
                    Rigidbody2D rb = newBomb.GetComponent<Rigidbody2D>();
                    Vector2 tempDirection = rb.velocity.normalized;
                    float newSpeed = rb.velocity.magnitude * 0.4f;
                    rb.AddForce(new Vector2(-tempDirection.x, -tempDirection.y) * newSpeed);
                }
                spaceBombManager.addBomb(newBomb);
                newBomb.GetComponent<SpaceBomb>().startExplosion();
            }
        }
    }

    private Vector2 getDirection()
    {
        float newAngle = (leftSpawner) ? (Random.Range(angleMin, angleMax) + 180) : Random.Range(angleMin, angleMax);
        float randomAngle = newAngle * Mathf.Deg2Rad;  // Convert angle to radians
        return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }

    public void resetSpawner()
    {
        StopAllCoroutines();
    }
}
