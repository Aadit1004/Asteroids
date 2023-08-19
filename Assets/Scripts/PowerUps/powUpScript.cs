using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powUpScript : MonoBehaviour
{
    [SerializeField] private GameObject powerUpManagerObj;
    private PowerUpsManager powerUpsManager;

    // Start is called before the first frame update
    void Start()
    {
        powerUpsManager = powerUpManagerObj.GetComponent<PowerUpsManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary")
        {
            powerUpsManager.removePowerUp(this.gameObject);
            Destroy(this.gameObject);
        }
    }

}
