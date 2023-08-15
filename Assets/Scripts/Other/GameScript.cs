using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public enum GameMode
{
    Classic,
    TimeAttack1,
    TimeAttack3,
    TimeAttack5,
    Survival
}

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

    private GameMode currentMode = GameMode.Classic; // classic by default for start up
    private const float timeAttack1TimeLimit = 60f;
    private const float timeAttack3TimeLimit = 180f;
    private const float timeAttack5TimeLimit = 300f;
    [SerializeField] private TMP_Text timerText;
    private float timeRemaining;

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
        // timer text
    }

    public bool isGameActive()
    {
        return isGameOn;
    }

    public void startGame(GameMode gameMode)
    {
        currentMode = gameMode;
        lives = (gameMode == GameMode.Survival) ? 1 : 3;
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
        if (gameMode == GameMode.TimeAttack1)
        {
            timerText.gameObject.SetActive(true);
            StartTimer(timeAttack1TimeLimit);
        } 
        else if (gameMode == GameMode.TimeAttack3)
        {
            timerText.gameObject.SetActive(true);
            StartTimer(timeAttack3TimeLimit);
        } 
        else if (gameMode == GameMode.TimeAttack5)
        {
            timerText.gameObject.SetActive(true);
            StartTimer(timeAttack5TimeLimit);
        }
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timeRemaining > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
            timerText.text = $"Time left: {timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
        timerText.text = "Time left: 00:00";
    }

    private void resetGame()
    {
        isGameOn = false;
        timerText.gameObject.SetActive(false);
        spaceShip.gameObject.SetActive(false);
        asteroidManager.clearAsteroidsList();
        for (int i = 0; i < Spawners.Length; i++)
        {
            AsteroidSpawner spawner = Spawners[i].GetComponent<AsteroidSpawner>();
            spawner.resetSpawner();
        }
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
        if (lives == 0 || (isGameActive() && timeRemaining == 0f))
        {
            // death animation and sound of ship
            updateScores(score, currentMode);
            endScreenScoreText.text = "Score: " + score;
            String highScoreText = String.Empty;
            switch (currentMode)
            {
                case GameMode.Classic:
                    highScoreText = highscores[0].ToString();
                    break;
                case GameMode.TimeAttack1:
                    highScoreText = highscores[1].ToString();
                    break;
                case GameMode.TimeAttack3:
                    highScoreText = highscores[2].ToString();
                    break;
                case GameMode.TimeAttack5:
                    highScoreText = highscores[3].ToString();
                    break;
                case GameMode.Survival:
                    highScoreText = highscores[4].ToString();
                    break;
            }
            endScreenHighestScoreText.text = "Highest Score: " + highScoreText;
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
        PlayerPrefs.SetString("classicScore", highscores[0].ToString());
        PlayerPrefs.SetString("timeScore1", highscores[1].ToString());
        PlayerPrefs.SetString("timeScore3", highscores[2].ToString());
        PlayerPrefs.SetString("timeScore5", highscores[3].ToString());
        PlayerPrefs.SetString("survivalScore", highscores[4].ToString());
        PlayerPrefs.Save();
    }

    public void loadData()
    {
        if (PlayerPrefs.HasKey("classicScore"))
        {
            highscores[0] = int.Parse(PlayerPrefs.GetString("classicScore"));
        }
        if (PlayerPrefs.HasKey("timeScore1"))
        {
            highscores[1] = int.Parse(PlayerPrefs.GetString("timeScore1"));
        }
        if (PlayerPrefs.HasKey("timeScore3"))
        {
            highscores[2] = int.Parse(PlayerPrefs.GetString("timeScore3"));
        }
        if (PlayerPrefs.HasKey("timeScore5"))
        {
            highscores[3] = int.Parse(PlayerPrefs.GetString("timeScore5"));
        }
        if (PlayerPrefs.HasKey("survivalScore"))
        {
            highscores[4] = int.Parse(PlayerPrefs.GetString("survivalScore"));
        }
        updateScoresUi();
    }

    public void updateScores(int newScore, GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Classic:
                if (newScore > highscores[0]) highscores[0] = newScore;
                break;
            case GameMode.TimeAttack1:
                if (newScore > highscores[1]) highscores[1] = newScore;
                break;
            case GameMode.TimeAttack3:
                if (newScore > highscores[2]) highscores[2] = newScore;
                break;
            case GameMode.TimeAttack5:
                if (newScore > highscores[3]) highscores[3] = newScore;
                break;
            case GameMode.Survival:
                if (newScore > highscores[4]) highscores[4] = newScore;
                break;
        }
        updateScoresUi();
    }

    public void updateScoresUi()
    {
        // O(n) runtime to update text scores
        for (int i = 0; i < highscores.Length; i++)
        {
            string scoreText = string.Empty;
            switch (i)
            {
                case 0:
                    scoreText = "Classic: " + highscores[i];
                    break;
                case 1:
                    scoreText = "Time Attack (1 minute): " + highscores[i];
                    break;
                case 2:
                    scoreText = "Time Attack (3 minutes): " + highscores[i];
                    break;
                case 3:
                    scoreText = "Time Attack (5 minutes): " + highscores[i];
                    break;
                case 4:
                    scoreText = "Survival: " + highscores[i];
                    break;
            }
            highscoreUiParent.transform.GetChild(i).GetComponent<TMP_Text>().text = scoreText;
        }
    }

    public GameMode getCurrentMode()
    {
        return currentMode;
    }
}
