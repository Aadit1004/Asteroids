using System;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    private bool isGameOn = false;
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private GameObject inGameUi;
    private int lives = 3;
    private int score = 0;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = spaceShip.transform.position;
        startRotation = spaceShip.transform.rotation;
    }

    private void FixedUpdate()
    {
        updateInGameTexts();
        checkLives();
    }

    private void updateInGameTexts()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives " + lives;
    }

    public bool isGameActive()
    {
        return isGameOn;
    }

    public void startGame()
    {
        isGameOn = true;
        spaceShip.gameObject.SetActive(true);
        resetShipPosition();
    }

    private void resetGame()
    {
        isGameOn = false;
        spaceShip.gameObject.SetActive(false);
        resetShipPosition();
        lives = 3;
    }

    private void resetShipPosition()
    {
        spaceShip.transform.position = startPosition;
        spaceShip.transform.rotation = startRotation;
    }

    private void checkLives()
    {
        if (lives == 0)
        {
            // death animation and sound of ship
            resetGame();
            inGameUi.gameObject.SetActive(false);
            mainMenuUi.SetActive(true);
        }
    }

    public void hitAsteroid()
    {
        lives--;
    }
}
