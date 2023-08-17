using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blackHoleObj;
    [SerializeField] private GameObject blackHoleManagerObj;
    private BlackHoleManager blackHoleManager;



    // degrees (as unit circle {0/360 to right, 180 to left})
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    // Start is called before the first frame update
    void Start()
    {
        blackHoleManager = blackHoleManagerObj.GetComponent<BlackHoleManager>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSpawner()
    {
        StartCoroutine(shootBlackHole());
    }

    private IEnumerator shootBlackHole()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(40f, 50f)); // wait time
            asteroidManager.setMaxAsteroids(3);
            if (blackHoleManager.getNumBlackHoles() < blackHoleManager.getMaxBlackHoles())
            {
                Vector3 spawnPos = transform.position;
                GameObject newBlackHole = Instantiate(blackHoleObj, spawnPos, Quaternion.identity);

                Vector2 direction = getDirection();
                float bombSpeed = 1f;
                newBlackHole.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * bombSpeed;
                newBlackHole.GetComponent<Rigidbody2D>().AddTorque(Random.Range(10, 20));

                blackHoleManager.addBlackHole(newBlackHole);
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
