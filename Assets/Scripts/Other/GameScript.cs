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

    private int[] highscores = new int[5];
    [SerializeField] private GameObject highscoreUiParent;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = spaceShip.transform.position;
        startRotation = spaceShip.transform.rotation;
        asteroidManager = AsteroidManagerObj.GetComponent<AsteroidManager>();
        for (int i = 0; i < highscores.Length; i++) highscores[i] = 0;
        loadData();
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
        hideCursor();
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
            updateScores(score);
            endScreenScoreText.text = "Score: " + score;
            endScreenHighestScoreText.text = "Highest Score: " + highscores[0];
            resetGame();
            inGameUi.gameObject.SetActive(false);
            endScreen.SetActive(true);
            showCursor();
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
        yield return new WaitForSeconds(2f);
        resetShipPosition();
        if (isGameActive()) spaceShip.gameObject.SetActive(true);
    }

    public void hideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void showCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void saveData()
    {
        PlayerPrefs.SetString("score1", highscores[0].ToString());
        PlayerPrefs.SetString("score2", highscores[1].ToString());
        PlayerPrefs.SetString("score3", highscores[2].ToString());
        PlayerPrefs.SetString("score4", highscores[3].ToString());
        PlayerPrefs.SetString("score5", highscores[4].ToString());
        PlayerPrefs.Save();
    }

    public void loadData()
    {
        if (PlayerPrefs.HasKey("score1"))
        {
            highscores[0] = int.Parse(PlayerPrefs.GetString("score1"));
        }
        if (PlayerPrefs.HasKey("score2"))
        {
            highscores[1] = int.Parse(PlayerPrefs.GetString("score2"));
        }
        if (PlayerPrefs.HasKey("score3"))
        {
            highscores[2] = int.Parse(PlayerPrefs.GetString("score3"));
        }
        if (PlayerPrefs.HasKey("score4"))
        {
            highscores[3] = int.Parse(PlayerPrefs.GetString("score4"));
        }
        if (PlayerPrefs.HasKey("score5"))
        {
            highscores[4] = int.Parse(PlayerPrefs.GetString("score5"));
        }
        updateScoresUi();
    }

    public void updateScores(int newScore)
    {
        if (newScore <= highscores[4]) return;
        // O(n) runtime
        for (int i = 0; i < highscores.Length; i++)
        {
            if (newScore > highscores[i])
            {
                // shift elements down
                for (int j = highscores.Length - 2; j > i; j--)
                {
                    highscores[j + 1] = highscores[j];
                }
                //update new scores
                highscores[i] = newScore;
                break;
            }
        }
        updateScoresUi();
    }

    public void updateScoresUi()
    {
        // O(n) runtime to update text scores
        for (int i = 0; i < highscores.Length; i++)
        {
            highscoreUiParent.transform.GetChild(i).GetComponent<TMP_Text>().text = (i + 1).ToString() + ". " + highscores[i];
        }
    }
}
