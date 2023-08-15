using System;
using System.Collections;
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

    public GameObject[] Spawners;

    [SerializeField] private GameObject AsteroidManagerObj;
    private AsteroidManager asteroidManager;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = spaceShip.transform.position;
        startRotation = spaceShip.transform.rotation;
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
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
        // start spawners
        for (int i = 0; i < Spawners.Length; i++)
        {
            AsteroidSpawner spawner = Spawners[i].GetComponent<AsteroidSpawner>();
            spawner.startSpawner();
        }
    }

    private void resetGame()
    {
        isGameOn = false;
        spaceShip.gameObject.SetActive(false);
        asteroidManager.clearAsteroidsList();
        for (int i = 0; i < Spawners.Length; i++)
        {
            AsteroidSpawner spawner = Spawners[i].GetComponent<AsteroidSpawner>();
            spawner.resetSpawner();
        }
        // remove all asteroids
        resetShipPosition();
        lives = 3;
        score = 0;
    }

    private void resetShipPosition()
    {
        spaceShip.transform.position = startPosition;
        spaceShip.transform.rotation = startRotation;
    }

    [SerializeField] private GameObject endScreen;
    public TMP_Text endScreenScoreText;
    public TMP_Text endScreenHighestScoreText;

    private void checkLives()
    {
        if (lives == 0)
        {
            // death animation and sound of ship
            endScreenScoreText.text = "Score: " + score;
            endScreenHighestScoreText.text = "Highest Score: " + 0;
            resetGame();
            inGameUi.gameObject.SetActive(false);
            endScreen.SetActive(true);
        }
    }

    public void hitAsteroid()
    {
        lives--;
    }
    public void hitBigAsteroid()
    {
        score += 100;
    }
    public void hitSmallAsteroid()
    {
        score += 25;
    }

    public void respawnShip()
    {
        spaceShip.gameObject.SetActive(false);
        StartCoroutine(delayRespawn());
    }
    private IEnumerator delayRespawn()
    {
        yield return new WaitForSeconds(3f);
        resetShipPosition();
        if (isGameActive()) spaceShip.gameObject.SetActive(true);
    }
}
