using System;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenPresenters
{
    public class PauseMenuScreenPresenter
    {
        private readonly IPauseMenuScreenView _view;
        private readonly SceneFader _sceneFader;
        private readonly GameManager _gameManager;

        private readonly string _menuSceneName = ConstantData.MenuSceneName;

        public PauseMenuScreenPresenter(
            IPauseMenuScreenView view,
            SceneFader sceneFader,
            GameManager gameManager)
        {
            _view = view;
            _sceneFader = sceneFader;
            _gameManager = gameManager;

            Initialize();
        }

        private void Initialize()
        {
            _view.OnShowHideGamePauseButtonClick += delegate
            {
                _gameManager.IsPauseActive = false;
            };

            _view.OnGoToMenuButtonClick += delegate
            {
                _gameManager.IsPauseActive = false;
                _sceneFader.FadeTo(_menuSceneName);
            };

            _view.OnRetryLevelButtonClick += delegate
            {
                _gameManager.IsPauseActive = false;
                _sceneFader.FadeTo(SceneManager.GetActiveScene().name);
            };
        }
        
        public void ShowHideGamePause(bool isActive)
        {
            _view.GetGameObject().SetActive(isActive);    
        }
    }

    public interface IPauseMenuScreenView
    {
        event Action OnShowHideGamePauseButtonClick;
        event Action OnGoToMenuButtonClick;
        event Action OnRetryLevelButtonClick;

        GameObject GetGameObject();
    }
}