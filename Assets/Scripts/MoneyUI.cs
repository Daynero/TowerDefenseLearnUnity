using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    
    private void Update()
    {
        moneyText.text = "$" + PlayerStats.PlayerMoney;
    }
}
