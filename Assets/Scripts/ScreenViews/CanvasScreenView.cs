using ScreenPresenters;
using TMPro;
using UnityEngine;

namespace ScreenViews
{
    public class CanvasScreenView : MonoBehaviour, ICanvasScreenView
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text livesText;

        public void DisplayMoney(int money)
        {
            moneyText.text = "$" + money;
        }

        public void DisplayLives(int lives)
        {
            livesText.text = lives + " LIVES";
        }
    }
}
