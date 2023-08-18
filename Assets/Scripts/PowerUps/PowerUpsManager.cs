using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    private int maxPowerUps = 2;
    private int currentPowerUps;
    private List<GameObject> lofPowerUps = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        currentPowerUps = 0;   
    }

    public int getMaxPowerUps() { return maxPowerUps; }
    public void addPowerUp(GameObject powerUp)
    {
        currentPowerUps++;
        lofPowerUps.Add(powerUp);
    }
    public void removePowerUp(GameObject powerUp)
    {
        currentPowerUps--;
        lofPowerUps.Remove(powerUp);
    }
    public int getNumPowerUps() { return currentPowerUps; }
    public void clearPowerUpsList()
    {
        foreach (var powerUp in lofPowerUps)
        {
            Destroy(powerUp);
        }
        lofPowerUps.Clear();
    }

}
