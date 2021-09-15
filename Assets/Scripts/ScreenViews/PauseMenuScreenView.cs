using System;
using ScreenPresenters;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScreenView : MonoBehaviour, IPauseMenuScreenView
{
    [SerializeField] private Button showHideGamePauseButton;
    [SerializeField] private Button retryLevelButton;
    [SerializeField] private Button goToMenuButton;

    public event Action onRetryLevelButtonClick;
    public event Action onGoToMenuButtonClick;
    public event Action onShowHideGamePauseButtonClick;

    private void Start()
    {
        showHideGamePauseButton.onClick.AddListener(delegate { onShowHideGamePauseButtonClick?.Invoke(); });

        retryLevelButton.onClick.AddListener(delegate { onRetryLevelButtonClick?.Invoke(); });

        goToMenuButton.onClick.AddListener(delegate { onGoToMenuButtonClick?.Invoke(); });
    }
}