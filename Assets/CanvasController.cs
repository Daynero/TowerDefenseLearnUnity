using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    private Enemy enemy;  
    private void Start()
    {
        PlayerStats.instance.MoneyUpdateNotify += DisplayMoney;
    }

    private void DisplayMoney(int money)
    {
        moneyText.text = "$" + money;
    }
}
