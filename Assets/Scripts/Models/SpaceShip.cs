using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    private Rigidbody2D rb;
    private float rotationAmount = 40f;
    private float thrustAmount = 1.3f;
    private const float maxThrust = 5;
    private float maxAngularVelocity = 200f;

    [SerializeField] private GameObject missile;
    private float missileSpeed = 8f;
    private float spawnDistance = 0.8f;

    [SerializeField] private GameObject gameManagerObject;

    private GameScript gameManager;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        gameManager = gameManagerObject.GetComponent<GameScript>();
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
    }

    void Update()
    {
        if (gameManager.isGameActive())
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    // If the current angular velocity exceeds the maximum, set the angular velocity to the maximum, preserving the sign
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                } else
                {
                    rb.AddTorque(-rotationAmount * Time.fixedDeltaTime);
                }
            } // rotate right
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                }
                else
                {
                    rb.AddTorque(rotationAmount * Time.fixedDeltaTime);
                }
            } // rotate left
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                //transform.position = (Vector2)transform.position + GetFacingDirection() * distance;
                if (rb.velocity.magnitude > maxThrust)
                {
                    rb.velocity = rb.velocity.normalized * maxThrust;    
                } 
                else
                {
                    rb.AddForce(GetFacingDirection() * thrustAmount);
                }
            } // add thrust
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 spawnPosition = transform.position + transform.up * spawnDistance;
                GameObject newMissile = Instantiate(missile, spawnPosition, Quaternion.identity);
                newMissile.GetComponent<Rigidbody2D>().velocity = GetFacingDirection() * missileSpeed;
            } // fire missile
        }
    }

    private Vector2 GetFacingDirection()
    {
        float radians = (90 + transform.eulerAngles.z) * Mathf.Deg2Rad; // Convert rotation to radians
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // Return unit vector
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Boundary")
        {
            //Debug.Log("Collided with boundary");
        }
        if ((other.tag == "AsteroidBig" || other.tag == "AsteroidSmall") && gameManager.canCollideWithThreats())
        {
            gameManager.hitAsteroid();
            asteroidManager.removeAsteroid(other);
            //Debug.Log("life lost");
            Destroy(other.gameObject);
            gameManager.respawnShip();
            // any logic for hitting asteroids
        }
    }
}