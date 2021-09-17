using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text livesText;
    
    private GameManager _gameManager;

    public CanvasController(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        _gameManager.MoneyUpdateNotify += DisplayMoney;
        _gameManager.LivesUpdateNotify += DisplayLives;
    }

    public void Init()
    {
        
    } 

    private void DisplayMoney(int money)
    {
        moneyText.text = "$" + money;
    }

    private void DisplayLives(int lives)
    {
        livesText.text = lives + " LIVES";
    }
}
