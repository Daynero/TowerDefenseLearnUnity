using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text livesText;
    
    private Enemy enemy;  
    private void Start()
    {
        PlayerStats.instance.MoneyUpdateNotify += DisplayMoney;
        PlayerStats.instance.LivesUpdateNotify += DisplayLives;
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
