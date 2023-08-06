using UnityEngine;

public class GameScript : MonoBehaviour
{
    private bool isGameOn = false;
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;
    [SerializeField] private GameObject spaceShip;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = spaceShip.transform.position;
        startRotation = spaceShip.transform.rotation;
    }

    public bool isGameActive()
    {
        return isGameOn;
    }

    public void startGame()
    {
        isGameOn = true;
        spaceShip.gameObject.SetActive(true);
        spaceShip.transform.position = startPosition;
        spaceShip.transform.rotation = startRotation;
    }
}
