using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    private GameScript gameScript;


    // Main UI gameObjs
    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private GameObject inGameUi;
    [SerializeField] private GameObject endScreenUi;
    [SerializeField] private GameObject highScoreUi;
    [SerializeField] private GameObject gameModesUi;


    // Main Menu Button Texts
    [SerializeField] private TMP_Text playGameButtonText;
    [SerializeField] private TMP_Text highScoreButtonText;
    [SerializeField] private TMP_Text creditsButtonText;


    // Game Modes Button Texts
    [SerializeField] private TMP_Text classicButtonText;
    [SerializeField] private TMP_Text time1ButtonText;
    [SerializeField] private TMP_Text time3ButtonText;
    [SerializeField] private TMP_Text time5ButtonText;
    [SerializeField] private TMP_Text survivalButtonText;


    // Other
    [SerializeField] private GameObject newHighScoreObj;

    // Main Menu

    void Start()
    {
        gameScript = GameManager.GetComponent<GameScript>();
        playGameButtonText.text = "Play";
    }

    public void OnStartGame()
    {
        mainMenuUi.gameObject.SetActive(false);
        gameModesUi.gameObject.SetActive(true);

        // reset texts
        playGameButtonText.text = "Play";
        highScoreButtonText.text = "High Scores";
        creditsButtonText.text = "Credits";
        classicButtonText.text = "Classic";
        time1ButtonText.text = "Time Attack - 1 minute";
        time3ButtonText.text = "Time Attack - 3 minutes";
        time5ButtonText.text = "Time Attack - 5 minutes";
        survivalButtonText.text = "Survival";
    }

    public void OnApplicationQuit()
    {
        gameScript.saveData();
    }
    public void OnQuit()
    {
        gameScript.saveData();
        Application.Quit();
    }


    // End Screen

    public void OnPlayAgainButton()
    {
        endScreenUi.gameObject.SetActive(false);
        newHighScoreObj.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(false);
        inGameUi.gameObject.SetActive(true);
        gameScript.startGame(gameScript.getCurrentMode());
    }

    public void OnMainMenuButton()
    {
        endScreenUi.gameObject.SetActive(false);
        newHighScoreObj.gameObject.SetActive(false);
        inGameUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(true);
    }

    // High scores

    public void OnHighScore()
    {
        mainMenuUi.SetActive(false);
        highScoreUi.gameObject.SetActive(true);
        highScoreButtonText.text = "High Scores";
    }

    public void OnHighScoreMainMenu()
    {
        highScoreUi.gameObject.SetActive(false);
        mainMenuUi.SetActive(true);
    }

    // Game modes

    public void OnClassicMode()
    {
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.Classic);
        inGameUi.gameObject.SetActive(true);
        classicButtonText.text = "Classic";
    }
    public void OnTimeAttack1Mode()
    {
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.TimeAttack1);
        inGameUi.gameObject.SetActive(true);
        time1ButtonText.text = "Time Attack - 1 minute";
    }
    public void OnTimeAttack3Mode()
    {
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.TimeAttack3);
        inGameUi.gameObject.SetActive(true);
        time3ButtonText.text = "Time Attack - 3 minutes";
    }
    public void OnTimeAttack5Mode()
    {
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.TimeAttack5);
        inGameUi.gameObject.SetActive(true);
        time5ButtonText.text = "Time Attack - 5 minutes";
    }
    public void OnSurvivalMode()
    {
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.Survival);
        inGameUi.gameObject.SetActive(true);
        survivalButtonText.text = "Survival";
    }
    public void OnModesMainMenu()
    {
        gameModesUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(true);
    }
}
