using System;
using Core;
using ScreenPresenters;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private int levelToUnlock = 2;
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private int startMoney = 400;
    [SerializeField] private int startLives = 20;

    private string nextLevel = ConstantData.Level2;
    private int playerMoney;
    private int playerLives;
    private int rounds;
    private bool isPauseActive = false;
    private PauseMenuScreenPresenter _pauseMenuScreenPresenter;
    private GameOverScreenPresenter _gameOverScreenPresenter;
    private WaveSpawner _waveSpawner;

    public static bool gameIsOver;
    
    public event Action<int> MoneyUpdateNotify;
    public event Action<int> LivesUpdateNotify;

    public int Rounds
    {
        private set => rounds = value;
        get => rounds;
    }
    
    public int PlayerMoney
    {
        get => playerMoney;
        set
        {
            playerMoney = value;
            MoneyUpdateNotify?.Invoke(playerMoney);
        }
    }

    private int PlayerLives
    {
        get => playerLives;
        set
        {
            playerLives = value;
            LivesUpdateNotify?.Invoke(playerLives);
        }
    }
    
    public void Initialize(PauseMenuScreenPresenter pauseMenuScreenPresenter,
                            GameOverScreenPresenter gameOverScreenPresenter,
                            WaveSpawner waveSpawner)
    {
        _pauseMenuScreenPresenter = pauseMenuScreenPresenter;
        _gameOverScreenPresenter = gameOverScreenPresenter;
        _waveSpawner = waveSpawner;
        
        _waveSpawner.EnemyDie = delegate(int enemyCost)
        {
            PlayerMoney += enemyCost;
        };
        _waveSpawner.EnemyPathEnded = delegate { PlayerLives--; };
    }

    private void Start()
    {
        _waveSpawner.RoundsUpdateNotify += UpdateRounds;
        
        PlayerMoney = startMoney;
        PlayerLives = startLives;

        Rounds = 0;
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

        if (PlayerLives <= 0)
        {
            EndGame();
        }
    }

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

    private void UpdateRounds()
    {
        rounds++;
        _gameOverScreenPresenter.SetRoundsCount(rounds);
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