using System;
using ScreenPresenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenViews
{
    public class MainGameScreenView : MonoBehaviour, IMainGameScreenView
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private Button selectDefaultTurretButton;
        [SerializeField] private Button selectMissileLauncherButton;
        [SerializeField] private Button selectLaserBeamerButton;

        public event Action<TurretType> OnTurretSelect;

        private void Start()
        {
            selectDefaultTurretButton.onClick.AddListener(delegate
            {
                OnTurretSelect?.Invoke(TurretType.DefaultTurret);
            });
            selectMissileLauncherButton.onClick.AddListener(delegate
            {
                OnTurretSelect?.Invoke(TurretType.MissileLauncher);
            });
            selectLaserBeamerButton.onClick.AddListener(delegate
            {
                OnTurretSelect?.Invoke(TurretType.LaserBeamer);
            });
        }

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
