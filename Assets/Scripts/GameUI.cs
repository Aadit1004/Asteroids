using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    private GameScript gameScript;

    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private GameObject inGameUi;

    // Start is called before the first frame update
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
}
