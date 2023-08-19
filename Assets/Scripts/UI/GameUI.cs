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
    [SerializeField] private GameObject creditsUi;


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

    // Sounds

    [SerializeField] private GameObject gameModeButtonObj;
    private AudioSource gameModeButtonSoundEffect;

    [SerializeField] private GameObject UiButtonObj;
    private AudioSource UiButtonSoundEffect;

    public AudioSource mainMenuSoundMusic;

    void Start()
    {
        gameScript = GameManager.GetComponent<GameScript>();
        playGameButtonText.text = "Play";
        gameModeButtonSoundEffect = gameModeButtonObj.GetComponent<AudioSource>();
        UiButtonSoundEffect = UiButtonObj.GetComponent<AudioSource>();
        FadeInMusic();
    }


    // Main Menu
    public void OnStartGame()
    {
        UiButtonSoundEffect.Play();
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
    } // calls game modes,,,doesnt start actual game

    public void OnApplicationQuit()
    {
        gameScript.saveData();
    }
    public void OnQuit()
    {
        UiButtonSoundEffect.Play();
        gameScript.saveData();
        Application.Quit();
    }
    public void OnHighScore()
    {
        UiButtonSoundEffect.Play();
        mainMenuUi.SetActive(false);
        highScoreUi.gameObject.SetActive(true);
        highScoreButtonText.text = "High Scores";
    }
    public void OnCredits()
    {
        UiButtonSoundEffect.Play();
        mainMenuUi.gameObject. SetActive(false);
        creditsUi.gameObject.SetActive(true);
        creditsButtonText.text = "Credits";
    }

    // End Screen

    public void OnPlayAgainButton()
    {
        gameModeButtonSoundEffect.Play();
        endScreenUi.gameObject.SetActive(false);
        newHighScoreObj.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(false);
        inGameUi.gameObject.SetActive(true);
        gameScript.startGame(gameScript.getCurrentMode());
    }

    public void OnMainMenuButton()
    {
        UiButtonSoundEffect.Play();
        endScreenUi.gameObject.SetActive(false);
        newHighScoreObj.gameObject.SetActive(false);
        inGameUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(true);
        FadeInMusic();
    }

    // High scores

    public void OnHighScoreMainMenu()
    {
        UiButtonSoundEffect.Play();
        highScoreUi.gameObject.SetActive(false);
        mainMenuUi.SetActive(true);
    }

    // Game modes

    public void OnClassicMode()
    {
        gameModeButtonSoundEffect.Play();
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.Classic);
        inGameUi.gameObject.SetActive(true);
        classicButtonText.text = "Classic";
        FadeOutMusic();
    }
    public void OnTimeAttack1Mode()
    {
        gameModeButtonSoundEffect.Play();
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.TimeAttack1);
        inGameUi.gameObject.SetActive(true);
        time1ButtonText.text = "Time Attack - 1 minute";
        FadeOutMusic();
    }
    public void OnTimeAttack3Mode()
    {
        gameModeButtonSoundEffect.Play();
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.TimeAttack3);
        inGameUi.gameObject.SetActive(true);
        time3ButtonText.text = "Time Attack - 3 minutes";
        FadeOutMusic();
    }
    public void OnTimeAttack5Mode()
    {
        gameModeButtonSoundEffect.Play();
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.TimeAttack5);
        inGameUi.gameObject.SetActive(true);
        time5ButtonText.text = "Time Attack - 5 minutes";
        FadeOutMusic();
    }
    public void OnSurvivalMode()
    {
        gameModeButtonSoundEffect.Play();
        gameModesUi.gameObject.SetActive(false);
        gameScript.startGame(GameMode.Survival);
        inGameUi.gameObject.SetActive(true);
        survivalButtonText.text = "Survival";
        FadeOutMusic();
    }
    public void OnModesMainMenu()
    {
        UiButtonSoundEffect.Play();
        gameModesUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(true);
    }


    // Credits 

    public void OnCreditsMainMenuButton()
    {
        UiButtonSoundEffect.Play();
        creditsUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(true);
    }

    // Music

    public void FadeInMusic()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 0.7f;
        float fadeDuration = 2f;

        mainMenuSoundMusic.volume = 0f;
        mainMenuSoundMusic.Play();

        while (mainMenuSoundMusic.volume < targetVolume)
        {
            mainMenuSoundMusic.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        mainMenuSoundMusic.volume = targetVolume;
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f;
        float startVolume = mainMenuSoundMusic.volume;

        while (mainMenuSoundMusic.volume > 0f)
        {
            mainMenuSoundMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        mainMenuSoundMusic.Stop();
        mainMenuSoundMusic.volume = 0f;
    }
}
