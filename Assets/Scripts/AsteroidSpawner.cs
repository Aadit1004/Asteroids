using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bigAsteroid;
    [SerializeField] private GameObject smallAsteroid;
    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;
    [SerializeField] private Vector2 angle1;
    [SerializeField] private Vector2 angle2;

    // Start is called before the first frame update
    void Start()
    {
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSpawner()
    {
        StartCoroutine(shootAsteroid());
    }

    private IEnumerator shootAsteroid()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (asteroidManager.getAsteroids() == 0 || (asteroidManager.getAsteroids() - 2 < asteroidManager.getMaxAsteroids())) // added leeway
            {
                asteroidManager.addAsteroid();
                int astVal = Random.Range(0, 2);
                GameObject ast = (astVal == 0) ? bigAsteroid : smallAsteroid;
                Vector3 spawnPos = transform.position;
                GameObject newAst = Instantiate(ast, spawnPos, Quaternion.identity);
                Vector2 direction = getDirection();
                newAst.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * Random.Range(1, 5);
                newAst.GetComponent<Rigidbody2D>().AddTorque(Random.Range(10, 40));
            }
        }
    }

    private Vector2 getDirection()
    {
        return new Vector2(Random.Range(angle1.x, angle2.x), Random.Range(angle1.y, angle2.y));
    }

    public void resetSpawner()
    {
        StopAllCoroutines();
    }
}
