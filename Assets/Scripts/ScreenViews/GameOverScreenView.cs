using System;
using ScreenPresenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenViews
{
    public class GameOverScreenView : MonoBehaviour, IGameOverScreenView
    {
        [SerializeField] private TMP_Text roundsText;
        [SerializeField] private Button retryLevelButton;
        [SerializeField] private Button goToMenuButton;

        public event Action OnRetryLevelButtonClick;
        public event Action OnGoToMenuButtonClick;
    
        private void Start()
        {
            retryLevelButton.onClick.AddListener(delegate { OnRetryLevelButtonClick?.Invoke(); });
            goToMenuButton.onClick.AddListener(delegate { OnGoToMenuButtonClick?.Invoke(); });
        }

        public void SetRoundsAmount(int amount)
        {
            roundsText.text = amount.ToString();
        }
    }
}
