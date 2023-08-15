using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject gameManager;
    private GameScript gameScript;

    // Start is called before the first frame update
    void Awake()
    {
        gameScript = gameManager.GetComponent<GameScript>();
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
            gameScript.hitBigAsteroid();
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
        if (other.tag == "AsteroidSmall")
        {
            gameScript.hitSmallAsteroid();
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

    }
}
