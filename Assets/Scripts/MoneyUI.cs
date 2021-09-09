using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text moneyText;

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + PlayerStats.Money.ToString();
    }
}
