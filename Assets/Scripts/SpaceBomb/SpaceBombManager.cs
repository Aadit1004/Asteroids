using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBombManager : MonoBehaviour
{

    [Range(1, 2)] public int maxBombs;
    private int currentBombs;
    private List<GameObject> lofBomb = new List<GameObject>();

    void Start()
    {
        currentBombs = 0;
    }

    public int getMaxBombs() { return maxBombs; }
    public void addBomb(GameObject bomb)
    {
        currentBombs++;
        lofBomb.Add(bomb);
    }
    public void removeBomb(GameObject bomb)
    {
        currentBombs--;
        lofBomb.Remove(bomb);
    }
    public int getNumBombs() { return currentBombs; }
    public void clearBombList()
    {
        foreach (var bomb in lofBomb)
        {
            Destroy(bomb.gameObject);
        }
        lofBomb.Clear();
        currentBombs = 0;
    }

    public void setsSpaceBombSpeed()
    {
        foreach (var bomb in lofBomb)
        {
            Rigidbody2D rb = bomb.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = rb.velocity.normalized;
            float newSpeed = rb.velocity.magnitude * 1.2f;
            rb.AddForce(new Vector2(-direction.x, -direction.y) * newSpeed);
        }
    }

    public void resetSpaceBombSpeed()
    {
        foreach (var bomb in lofBomb)
        {
            Rigidbody2D rb = bomb.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = rb.velocity.normalized;
            float newSpeed = rb.velocity.magnitude * 1.2f;
            rb.AddForce(new Vector2(direction.x, direction.y) * newSpeed);
        }
    }
}
