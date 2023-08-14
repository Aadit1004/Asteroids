using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    private GameScript gameScript;

    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private GameObject inGameUi;
    [SerializeField] private GameObject endScreenUi;

    // Main Menu

    void Start()
    {
        gameScript = GameManager.GetComponent<GameScript>();
    }

    public void OnStartGame()
    {
        mainMenuUi.gameObject.SetActive(false);
        gameScript.startGame();
        inGameUi.gameObject.SetActive(true);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }


    // End Screen

    public void OnPlayAgainButton()
    {
        endScreenUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(false);
        inGameUi.gameObject.SetActive(true);
        gameScript.startGame();
    }

    public void OnMainMenuButton()
    {
        endScreenUi.gameObject.SetActive(false);
        inGameUi.gameObject.SetActive(false);
        mainMenuUi.gameObject.SetActive(true);
    }
}
