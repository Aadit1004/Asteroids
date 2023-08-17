using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bigAsteroid;
    [SerializeField] private GameObject smallAsteroid;
    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

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
            yield return new WaitForSeconds(Random.Range(2, 6)); // wait 2 to 5 seconds
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
                    newAst.GetComponent<Rigidbody2D>().AddTorque(Random.Range(10, 40));
                
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
