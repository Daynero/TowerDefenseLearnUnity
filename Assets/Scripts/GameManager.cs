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

    private readonly string _nextLevel = ConstantData.Level2;
    private int _playerMoney;
    private int _playerLives;
    private int _rounds;
    private bool _isPauseActive;
    private PauseMenuScreenPresenter _pauseMenuScreenPresenter;
    private GameOverScreenPresenter _gameOverScreenPresenter;
    private WaveSpawner _waveSpawner;
    private BuildManager _buildManager;

    public static bool GameIsOver;
    
    public event Action<int> MoneyUpdateNotify;
    public event Action<int> LivesUpdateNotify;

    private int Rounds
    {
        set => _rounds = value;
    }
    
    public int PlayerMoney
    {
        get => _playerMoney;
        private set
        {
            _playerMoney = value;
            MoneyUpdateNotify?.Invoke(_playerMoney);
        }
    }

    private int PlayerLives
    {
        get => _playerLives;
        set
        {
            _playerLives = value;
            LivesUpdateNotify?.Invoke(_playerLives);
        }
    }
    
    public void Initialize(PauseMenuScreenPresenter pauseMenuScreenPresenter,
                            GameOverScreenPresenter gameOverScreenPresenter,
                            WaveSpawner waveSpawner, BuildManager buildManager)
    {
        _pauseMenuScreenPresenter = pauseMenuScreenPresenter;
        _gameOverScreenPresenter = gameOverScreenPresenter;
        _waveSpawner = waveSpawner;
        _buildManager = buildManager;
        
        _waveSpawner.EnemyDie = delegate(int enemyCost)
        {
            PlayerMoney += enemyCost;
            _waveSpawner.EnemiesAlive--;
        };
        
        _buildManager.SellTurretAction = delegate(int salesMoney)
        {
            PlayerMoney += salesMoney;
        };
        
        _buildManager.BuildTurretAction = delegate(int turretCost)
        {
            if (PlayerMoney < turretCost)
            {
                Debug.Log("Not enough money to build that!");
                return;
            }

            PlayerMoney -= turretCost;
        };
        
        _buildManager.UpgradeTurretAction = delegate(int upgradeCost)
        {
            if (PlayerMoney < upgradeCost)
            {
                Debug.Log("Not enough money to upgrade that!");
                return;
            }

            PlayerMoney -= upgradeCost;
        };
        
        _waveSpawner.EnemyPathEnded += delegate { PlayerLives--; };
    }

    private void Start()
    {
        _waveSpawner.RoundsUpdateNotify += UpdateRounds;
        
        
        PlayerMoney = startMoney;
        PlayerLives = startLives;

        Rounds = 0;
        IsPauseActive = false;
        GameIsOver = false;
    }

    private void Update()
    {
        if (GameIsOver)
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
            _isPauseActive = value;
            Time.timeScale = _isPauseActive ? 0f : 1f;
            _pauseMenuScreenPresenter.ShowHideGamePause(_isPauseActive);
        }

        get => _isPauseActive;
    }

    private void UpdateRounds()
    {
        _rounds++;
        _gameOverScreenPresenter.SetRoundsCount(_rounds);
    }

    private void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        Debug.Log("LEVEL WON!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        PlayerPrefs.Save();
        sceneFader.FadeTo(_nextLevel);
    }
}