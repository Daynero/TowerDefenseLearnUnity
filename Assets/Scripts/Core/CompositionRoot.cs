using ScreenPresenters;
using UnityEngine;

namespace Core
{
    public class CompositionRoot : MonoBehaviour
    {
        private SceneFader _sceneFader;
        [SerializeField] private GameOverScreenView _gameOverScreenView;

        private GameOverScreenPresenter _gameOverScreenPresenter;
        private void Awake()
        {
            _sceneFader = FindObjectOfType<SceneFader>();
            _gameOverScreenPresenter = new GameOverScreenPresenter(_gameOverScreenView, _sceneFader);
        }
    }
}