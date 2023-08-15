using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Range(0, 20)] private int maxAsteroids;
    private int currentAsteroids = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getMaxAsteroids() { return  maxAsteroids; }
    public void addAsteroid() { currentAsteroids++; }
    public void removeAsteroid() { currentAsteroids--; }
    public int getAsteroids() { return currentAsteroids; }
}
