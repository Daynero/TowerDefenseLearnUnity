using System;
using ScreenPresenters;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenViews
{
    public class PauseMenuScreenView : MonoBehaviour, IPauseMenuScreenView
    {
        [SerializeField] private Button showHideGamePauseButton;
        [SerializeField] private Button retryLevelButton;
        [SerializeField] private Button goToMenuButton;

        public event Action OnRetryLevelButtonClick;
        public event Action OnGoToMenuButtonClick;
        public event Action OnShowHideGamePauseButtonClick;

        private void Start()
        {
            showHideGamePauseButton.onClick.AddListener(delegate { OnShowHideGamePauseButtonClick?.Invoke(); });

            retryLevelButton.onClick.AddListener(delegate { OnRetryLevelButtonClick?.Invoke(); });

            goToMenuButton.onClick.AddListener(delegate { OnGoToMenuButtonClick?.Invoke(); });
        }
    
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}