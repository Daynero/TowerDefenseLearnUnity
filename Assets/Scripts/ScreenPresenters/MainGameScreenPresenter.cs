using System;
using Data;

namespace ScreenPresenters
{
    public class MainGameScreenPresenter
    {
        private readonly GameManager _gameManager;
        private readonly IMainGameScreenView _mainGameScreenView;
        private readonly BuildManager _buildManager;

        public MainGameScreenPresenter(GameManager gameManager,
            IMainGameScreenView mainGameScreenView, BuildManager buildManager)
        {
            _gameManager = gameManager;
            _mainGameScreenView = mainGameScreenView;
            _buildManager = buildManager;

            Initialize();
        }

        private void Initialize()
        {
            _gameManager.MoneyUpdateNotify += _mainGameScreenView.DisplayMoney;
            _gameManager.LivesUpdateNotify += _mainGameScreenView.DisplayLives;
            _buildManager.DisplayCurrentTurretPrice = _mainGameScreenView.CurrentTurretPrice;
            
            _mainGameScreenView.OnTurretSelect += delegate(TurretType type)
            {
                _buildManager.SelectTurretToBuild(type);
            };
            
            
        }
    }

    public interface IMainGameScreenView
    {
        public event Action<TurretType> OnTurretSelect;
        public void DisplayMoney(int money);
        public void DisplayLives(int lives);
        public void CurrentTurretPrice(TurretInfoSO turretInfoSo);
    }
}