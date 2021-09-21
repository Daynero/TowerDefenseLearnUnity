using Data;
using ScreenPresenters;
using ScreenViews;
using UnityEngine;

namespace Core
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private GameOverScreenView gameOverScreenView;
        [SerializeField] private PauseMenuScreenView pauseMenuScreenView;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private WaveSpawner waveSpawner; 
        [SerializeField] private BuildManager buildManager; 
        [SerializeField] private MainGameScreenView mainGameScreenView;
        [SerializeField] private TurretInfoSO turretInfoSo;
        [SerializeField] private Node node;

        private SceneFader _sceneFader;
        private GameOverScreenPresenter _gameOverScreenPresenter;
        private PauseMenuScreenPresenter _pauseMenuScreenPresenter;
        private MainGameScreenPresenter _mainGameScreenPresenter;

        private void Awake()
        {
            _sceneFader = FindObjectOfType<SceneFader>();
            _gameOverScreenPresenter = new GameOverScreenPresenter(gameOverScreenView, _sceneFader);
            _pauseMenuScreenPresenter = new PauseMenuScreenPresenter(pauseMenuScreenView, _sceneFader, gameManager);
            _mainGameScreenPresenter = new MainGameScreenPresenter(gameManager, mainGameScreenView, buildManager);
            
            gameManager.Initialize(_pauseMenuScreenPresenter, _gameOverScreenPresenter, waveSpawner, buildManager);
            buildManager.Initialize(node, turretInfoSo);
            node.Initialize(buildManager);
        }
    }
}