using ScreenViews;

namespace ScreenPresenters
{
    public class CanvasScreenPresenter
    {
        private GameManager _gameManager;
        private CanvasScreenView _canvasScreenView;
        
        public CanvasScreenPresenter(GameManager gameManager,
            CanvasScreenView canvasScreenView)
        {
            _gameManager = gameManager;
            _canvasScreenView = canvasScreenView;

            Initialize();
        }
        
        public void Initialize()
        {
            _gameManager.MoneyUpdateNotify += _canvasScreenView.DisplayMoney;
            _gameManager.LivesUpdateNotify += _canvasScreenView.DisplayLives;
        }
    }

    public interface ICanvasScreenView
    {
        public void DisplayMoney(int money);
        public void DisplayLives(int lives);
    }
}