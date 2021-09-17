using System;
using ScreenPresenters;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameOverScreenView : MonoBehaviour, IGameOverScreenView
{
    [SerializeField] private TMP_Text roundsText;
    [SerializeField] private Button retryLevelButton;
    [SerializeField] private Button goToMenuButton;

    public event Action onRetryLevelButtonClick;
    public event Action onGoToMenuButtonClick;
    
    private void Start()
    {
        retryLevelButton.onClick.AddListener(delegate { onRetryLevelButtonClick?.Invoke(); });
        goToMenuButton.onClick.AddListener(delegate { onGoToMenuButtonClick?.Invoke(); });
    }

    public void SetRoundsAmount(int amount)
    {
        roundsText.text = amount.ToString();
    }
}
