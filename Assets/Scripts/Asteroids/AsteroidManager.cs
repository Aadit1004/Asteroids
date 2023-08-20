using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    private int maxAsteroids = 10;
    private int tempMax;
    private int currentAsteroids;
    private List<GameObject> lofAsteroid = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentAsteroids = 0;
        tempMax = maxAsteroids;
    }

    public int getMaxAsteroids() { return  maxAsteroids; }
    public void addAsteroid(GameObject asteroid) 
    { 
        currentAsteroids++;
        lofAsteroid.Add(asteroid);
    }
    public void removeAsteroid(GameObject asteroid) 
    { 
        currentAsteroids--;
        lofAsteroid.Remove(asteroid);
    }
    public int getAsteroids() { return currentAsteroids; }

    public void clearAsteroidsList()
    {
        foreach (var Asteroid in  lofAsteroid)
        {
            Destroy(Asteroid.gameObject);
        }
        lofAsteroid.Clear();
        currentAsteroids = 0;
    }

    // when black hole or space bomb is there -> reduce max asteroids
    public void setMaxAsteroids(int newMax) { if (newMax < maxAsteroids) maxAsteroids = newMax; }
    public void resetMaxAsteroids() { maxAsteroids = tempMax; }

    public void setAsteroidsSpeed()
    {
        foreach (var ast in lofAsteroid)
        {
            Rigidbody2D rb = ast.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = rb.velocity.normalized;
            float newSpeed = rb.velocity.magnitude * 1.2f;
            rb.AddForce(new Vector2(-direction.x, -direction.y) * newSpeed);
        }
    }

    public void resetAsteroidsSpeed()
    {
        foreach (var ast in lofAsteroid)
        {
            Rigidbody2D rb = ast.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = rb.velocity.normalized;
            float newSpeed = rb.velocity.magnitude * 1.2f;
            rb.AddForce(new Vector2(direction.x, direction.y) * newSpeed);
        }
    }
}
