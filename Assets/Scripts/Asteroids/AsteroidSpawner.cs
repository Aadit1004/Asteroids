using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bigAsteroid;
    [SerializeField] private GameObject smallAsteroid;
    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;
    [SerializeField] private GameObject spaceShip;

    // degrees (as unit circle {0/360 to right, 180 to left})
    [SerializeField] private float angleMin;  
    [SerializeField] private float angleMax;  

    // Start is called before the first frame update
    void Start()
    {
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
    }

    public void startSpawner()
    {
        StartCoroutine(shootAsteroid());
    }

    private IEnumerator shootAsteroid()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // wait time - (2f, 5f) default
            if (asteroidManager.getAsteroids() < asteroidManager.getMaxAsteroids())
            {
                int astVal = Random.Range(0, 2);
                GameObject ast = (astVal == 0) ? bigAsteroid : smallAsteroid; // random choice of small or big asteroids

                Vector3 spawnPos = transform.position;
                GameObject newAst = Instantiate(ast, spawnPos, Quaternion.identity);
                asteroidManager.addAsteroid(newAst);
                Vector2 direction = getDirection();
                float astSpeed = Random.Range(1f, 3f);
                newAst.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * astSpeed;
                float torqueMagnitude = Random.Range(10f, 40f);
                float torqueDirection = Random.Range(0, 2) == 0 ? 1 : -1;
                float finalTorque = torqueMagnitude * torqueDirection;
                newAst.GetComponent<Rigidbody2D>().AddTorque(finalTorque);
                if (spaceShip.gameObject.GetComponent<SpaceShip>().timeDilationIsActive())
                {
                    Rigidbody2D rb = newAst.GetComponent<Rigidbody2D>();
                    Vector2 tempDirection = rb.velocity.normalized;
                    float newSpeed = rb.velocity.magnitude * 1.2f;
                    rb.AddForce(new Vector2(-tempDirection.x, -tempDirection.y) * newSpeed);
                }
            }
        }
    }

    private Vector2 getDirection()
    {
        float randomAngle = Random.Range(angleMin, angleMax) * Mathf.Deg2Rad;  // Convert angle to radians
        return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }

    public void resetSpawner()
    {
        StopAllCoroutines();
    }
}
