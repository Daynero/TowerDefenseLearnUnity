using System;
using UnityEngine.SceneManagement;

namespace ScreenPresenters
{
    public class GameOverScreenPresenter
    {
        private IGameOverScreenView _view;
        private SceneFader _sceneFader;
        
        private string menuSceneName = "MainMenu";
        public GameOverScreenPresenter(
            GameOverScreenView view,
            SceneFader sceneFader)
        {
            _view = view;
            _sceneFader = sceneFader;
            Initialize();
        }

        private void Initialize()
        {
            _view.onRetryLevelButtonClick += delegate { _sceneFader.FadeTo(SceneManager.GetActiveScene().name); };
            _view.onGoToMenuButtonClick += delegate { _sceneFader.FadeTo(menuSceneName); };
        }
    }

    public interface IGameOverScreenView
    {
        event Action onRetryLevelButtonClick;
        event Action onGoToMenuButtonClick;
    }
}