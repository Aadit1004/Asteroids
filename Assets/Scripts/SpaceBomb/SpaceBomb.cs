using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBomb : MonoBehaviour
{
    private int lives = 2;
    private float bombTimer;

    [SerializeField] private GameObject bombManagerObj;
    private SpaceBombManager spaceBombManager;

    // Add timer


    void Start()
    {
        spaceBombManager = bombManagerObj.GetComponent<SpaceBombManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "AsteroidBoundary")
        {
            spaceBombManager.removeBomb(this.gameObject);
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
