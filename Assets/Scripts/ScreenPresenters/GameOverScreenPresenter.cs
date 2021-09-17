using Core;
using System;
using UnityEngine.SceneManagement;

namespace ScreenPresenters
{
    public class GameOverScreenPresenter
    {
        private IGameOverScreenView _view;
        private SceneFader _sceneFader;

        private readonly string _menuSceneName = ConstantData.menuSceneName;
        public GameOverScreenPresenter(
            IGameOverScreenView view,
            SceneFader sceneFader 
            )
        {
            _view = view;
            _sceneFader = sceneFader;

            Initialize();
        }

        private void Initialize()
        {
            _view.onRetryLevelButtonClick += delegate { _sceneFader.FadeTo(SceneManager.GetActiveScene().name); };
            _view.onGoToMenuButtonClick += delegate { _sceneFader.FadeTo(_menuSceneName); };
        }

        public void SetRoundsCount(int amount)
        {
            _view.SetRoundsAmount(amount);
        }
    }

    public interface IGameOverScreenView
    {
        event Action onRetryLevelButtonClick;
        event Action onGoToMenuButtonClick;
        public void SetRoundsAmount(int amount);
    }
}