using Core;
using System;
using UnityEngine.SceneManagement;

namespace ScreenPresenters
{
    public class GameOverScreenPresenter
    {
        private readonly IGameOverScreenView _view;
        private readonly SceneFader _sceneFader;

        private readonly string _menuSceneName = ConstantData.MenuSceneName;
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
            _view.OnRetryLevelButtonClick += delegate { _sceneFader.FadeTo(SceneManager.GetActiveScene().name); };
            _view.OnGoToMenuButtonClick += delegate { _sceneFader.FadeTo(_menuSceneName); };
        }

        public void SetRoundsCount(int amount)
        {
            _view.SetRoundsAmount(amount);
        }
    }

    public interface IGameOverScreenView
    {
        event Action OnRetryLevelButtonClick;
        event Action OnGoToMenuButtonClick;
        public void SetRoundsAmount(int amount);
    }
}