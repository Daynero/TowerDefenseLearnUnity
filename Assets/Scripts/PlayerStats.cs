using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int startMoney = 400;
    [SerializeField] private int startLives = 20;

    public event Action<int> MoneyUpdateNotify;
    private int playerMoney;
    public static PlayerStats instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int PlayerMoney
    {
        get
        {
            return playerMoney;
        }
        set
        {
            playerMoney = value;
            MoneyUpdateNotify?.Invoke(playerMoney);
        }
    
    }
    public static int PlayerLives;
    public static int Rounds;
    
    
    
    private void Start()
    {
        PlayerMoney = startMoney;
        PlayerLives = startLives;

        Rounds = 0;
    }
}
