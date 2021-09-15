using System;
using ScreenPresenters;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    
    [SerializeField] private string nextLevel = "Level02";
    [SerializeField] private int levelToUnlock = 2;
    [SerializeField] private SceneFader sceneFader;

    public static bool gameIsOver;
    
    private bool isPauseActive = false;
    private PauseMenuScreenPresenter _pauseMenuScreenPresenter;
    public bool IsPauseActive
    {
        set
        {
            isPauseActive = value;
            Time.timeScale = isPauseActive ? 0f : 1f;
            _pauseMenuScreenPresenter.ShowHideGamePause(isPauseActive);
        }

        get => isPauseActive;
    }

    public void Initialize(PauseMenuScreenPresenter pauseMenuScreenPresenter)
    {
        _pauseMenuScreenPresenter = pauseMenuScreenPresenter;
    }

    private void Start()
    {
        IsPauseActive = false;
        gameIsOver = false;
    }

    private void Update()
    {
        if (gameIsOver)
            return;

        if (Input.GetKeyDown("e"))
        {
            EndGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            IsPauseActive = !IsPauseActive;
        }

        if (PlayerStats.instance.PlayerLives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        Debug.Log("LEVEL WON!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        PlayerPrefs.Save();
        sceneFader.FadeTo(nextLevel);
    }
}