using System;
using Data;
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
        [SerializeField] private TMP_Text currentDefaultTurretPrice;
        [SerializeField] private TMP_Text currentMissileLauncherPrice;
        [SerializeField] private TMP_Text currentLaserBeamerPrice;

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
            selectLaserBeamerButton.onClick.AddListener(delegate { OnTurretSelect?.Invoke(TurretType.LaserBeamer); });
        }

        public void DisplayMoney(int money)
        {
            moneyText.text = "$" + money;
        }

        public void DisplayLives(int lives)
        {
            livesText.text = lives + " LIVES";
        }

        public void CurrentTurretPrice(TurretInfoSO turretInfoSo)
        {
            foreach (var item in turretInfoSo.turretArray)
            {
                switch (item.type)
                {
                    case TurretType.DefaultTurret:
                        currentDefaultTurretPrice.text = item.cost.ToString();
                        break;
                    case TurretType.MissileLauncher:
                        currentMissileLauncherPrice.text = item.cost.ToString();
                        break;
                    case TurretType.LaserBeamer:
                        currentLaserBeamerPrice.text = item.cost.ToString();
                        break;
                    default:
                        Debug.Log("something wrong");
                        break;
                }
            }
        }
    }
}