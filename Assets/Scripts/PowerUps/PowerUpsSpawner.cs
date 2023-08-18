using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject timeDilationObj;
    [SerializeField] private GameObject burstFireObj;
    [SerializeField] private GameObject shieldObj;
    [SerializeField] private GameObject powerUpManagerObj;
    private PowerUpsManager powerUpManager;

    // degrees (as unit circle {0/360 to right, 180 to left})
    [SerializeField] private float angleMin;
    [SerializeField] private float angleMax;

    public bool leftSpawner = false;

    // Start is called before the first frame update
    void Start()
    {
        powerUpManager = powerUpManagerObj.GetComponent<PowerUpsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSpawner()
    {
        StartCoroutine(shootPowerUp());
    }

    private IEnumerator shootPowerUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // wait time - (12f, 20f) default
            if (powerUpManager.getNumPowerUps() < powerUpManager.getMaxPowerUps())
            {
                Vector3 spawnPos = transform.position;
                GameObject powerupObj;
                float randomValue = Random.value;
                if (randomValue <= 0.4f) 
                {
                    powerupObj = shieldObj;
                } 
                else if (randomValue <= 0.70f) 
                {
                    powerupObj = burstFireObj;
                } 
                else 
                {
                    powerupObj = timeDilationObj;
                }
                GameObject newPowerUp = Instantiate(powerupObj, spawnPos, Quaternion.identity);

                Vector2 direction = getDirection();
                float powerUpSpeed = Random.Range(1.5f, 3f);
                newPowerUp.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * powerUpSpeed;
                float torqueMagnitude = Random.Range(10f, 20f);
                float torqueDirection = Random.Range(0, 2) == 0 ? 1 : -1;
                float finalTorque = torqueMagnitude * torqueDirection;
                newPowerUp.GetComponent<Rigidbody2D>().AddTorque(finalTorque);
                //if (spaceShip.gameObject.GetComponent<SpaceShip>().timeDilationIsActive())
                //{
                //    Rigidbody2D rb = newBomb.GetComponent<Rigidbody2D>();
                //    Vector2 tempDirection = rb.velocity.normalized;
                //    float newSpeed = rb.velocity.magnitude * 0.4f;
                //    rb.AddForce(new Vector2(-tempDirection.x, -tempDirection.y) * newSpeed);
                //}
                powerUpManager.addPowerUp(newPowerUp);
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
