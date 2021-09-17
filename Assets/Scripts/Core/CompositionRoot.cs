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
        [SerializeField] private CanvasScreenView canvasScreenView;
        // [SerializeField] private BuildManager buildManager;

        private SceneFader _sceneFader;
        private GameOverScreenPresenter _gameOverScreenPresenter;
        private PauseMenuScreenPresenter _pauseMenuScreenPresenter;
        private CanvasScreenPresenter _canvasScreenPresenter;

        private void Awake()
        {
            _sceneFader = FindObjectOfType<SceneFader>();
            _gameOverScreenPresenter = new GameOverScreenPresenter(gameOverScreenView, _sceneFader);
            _pauseMenuScreenPresenter = new PauseMenuScreenPresenter(pauseMenuScreenView, _sceneFader, gameManager);
            _canvasScreenPresenter = new CanvasScreenPresenter(gameManager, canvasScreenView);
            
            gameManager.Initialize(_pauseMenuScreenPresenter, _gameOverScreenPresenter, waveSpawner);
        }
    }
}