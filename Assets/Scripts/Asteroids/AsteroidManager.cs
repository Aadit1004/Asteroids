using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Range(10, 20)] public int maxAsteroids;
    private int currentAsteroids = 0;
    private List<GameObject> lofAsteroid = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentAsteroids = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
