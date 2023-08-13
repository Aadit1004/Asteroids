using UnityEngine;

public class Missile : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "Boundary")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "AsteroidBig")
        {
            // animation for defeating asteroid
            // update score accordingly
            Destroy(this.gameObject);
            Debug.Log("Hit big asteroid");
        }
        if (other.tag == "AsteroidSmall")
        {
            // animation for defeating asteroid
            // update score accordingly
            Destroy(this.gameObject);
            Debug.Log("Hit small asteroid");
        }
        // if other is asteroid, destroy it and add to score
        // destroy missle as well

    }
}
