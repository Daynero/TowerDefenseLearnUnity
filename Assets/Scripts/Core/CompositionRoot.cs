using ScreenPresenters;
using UnityEngine;

namespace Core
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private GameOverScreenView gameOverScreenView;
        [SerializeField] private PauseMenuScreenView pauseMenuScreenView;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private CanvasController canvasController;
        [SerializeField] private BuildManager buildManager;

        private SceneFader _sceneFader;
        private GameOverScreenPresenter _gameOverScreenPresenter;
        private PauseMenuScreenPresenter _pauseMenuScreenPresenter;

        private void Awake()
        {
            _sceneFader = FindObjectOfType<SceneFader>();
            _gameOverScreenPresenter = new GameOverScreenPresenter(gameOverScreenView, _sceneFader, gameManager);
            _pauseMenuScreenPresenter = new PauseMenuScreenPresenter(pauseMenuScreenView, _sceneFader, gameManager);
            
            gameManager.Initialize(_pauseMenuScreenPresenter, _gameOverScreenPresenter, waveSpawner);
        }
    }
}