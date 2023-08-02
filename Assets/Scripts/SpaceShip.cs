using Unity.VisualScripting;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    bool activeGame = true;

    private Rigidbody2D rb;
    private float rotationAmount = 40f;
    private float thrustAmount = 1.3f;
    private const float maxThrust = 5;
    private float maxAngularVelocity = 200f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        if (activeGame)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    // If the current angular velocity exceeds the maximum, set the angular velocity to the maximum, preserving the sign
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                } else
                {
                    rb.AddTorque(-rotationAmount * Time.fixedDeltaTime);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (Mathf.Abs(rb.angularVelocity) > maxAngularVelocity)
                {
                    // If the current angular velocity exceeds the maximum, set the angular velocity to the maximum, preserving the sign
                    rb.angularVelocity = maxAngularVelocity * Mathf.Sign(rb.angularVelocity);
                }
                else
                {
                    rb.AddTorque(rotationAmount * Time.fixedDeltaTime);
                }
            }
            if (Input.GetKey(KeyCode.W))
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
                
            }
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
    }
}