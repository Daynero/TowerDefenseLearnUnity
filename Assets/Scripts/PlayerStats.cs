using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 400;

    void Start()
    {
        Money = startMoney;
        Debug.Log("Money = " + Money.ToString());
    }
}
