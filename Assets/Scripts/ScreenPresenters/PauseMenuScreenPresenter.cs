using System;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScreenPresenters
{
    public class PauseMenuScreenPresenter
    {
        private IPauseMenuScreenView _view;
        private SceneFader _sceneFader;
        private GameManager _gameManager;

        private readonly string _menuSceneName = ConstantData.menuSceneName;

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
            _view.onShowHideGamePauseButtonClick += delegate
            {
                _gameManager.IsPauseActive = false;
            };

            _view.onGoToMenuButtonClick += delegate
            {
                _gameManager.IsPauseActive = false;
                _sceneFader.FadeTo(_menuSceneName);
            };

            _view.onRetryLevelButtonClick += delegate
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
        event Action onShowHideGamePauseButtonClick;
        event Action onGoToMenuButtonClick;
        event Action onRetryLevelButtonClick;

        GameObject GetGameObject();
    }
}