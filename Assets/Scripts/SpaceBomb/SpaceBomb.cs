using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBomb : MonoBehaviour
{
    private int lives = 2;

    // Add timer


    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary")
        {
            Destroy(this.gameObject);
        }
    }

    public int getLives()
    {
        return lives;
    }

    public void detuctLife()
    {
        lives--;
    }
}
