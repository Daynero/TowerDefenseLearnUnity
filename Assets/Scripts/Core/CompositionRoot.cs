using ScreenPresenters;
using UnityEngine;

namespace Core
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private GameOverScreenView gameOverScreenView;
        [SerializeField] private PauseMenuScreenView pauseMenuScreenView;
        [SerializeField] private GameManager gameManager;

        private SceneFader _sceneFader;
        private GameOverScreenPresenter _gameOverScreenPresenter;
        private PauseMenuScreenPresenter _pauseMenuScreenPresenter;
        
        private void Awake()
        {
            _sceneFader = FindObjectOfType<SceneFader>();
            _gameOverScreenPresenter = new GameOverScreenPresenter(gameOverScreenView, _sceneFader);
            _pauseMenuScreenPresenter = new PauseMenuScreenPresenter(pauseMenuScreenView, _sceneFader, gameManager);
        }
    }
}