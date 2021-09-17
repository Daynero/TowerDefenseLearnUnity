using System;
using UnityEngine;
using UnityEngine.Events;

// public class PlayerStats : MonoBehaviour
// {
//     [SerializeField] private int startMoney = 400;
//     [SerializeField] private int startLives = 20;
//
//     private int playerMoney;
//     private int playerLives;
//     
//     public event Action<int> MoneyUpdateNotify;
//     public event Action<int> LivesUpdateNotify;
//
//     //public static PlayerStats instance;
//     public int Rounds;
//     public int PlayerMoney
//     {
//         get => playerMoney;
//         set
//         {
//             playerMoney = value;
//             MoneyUpdateNotify?.Invoke(playerMoney);
//         }
//     
//     }
//
//     public int PlayerLives
//     {
//         get => playerLives;
//         set
//         {
//             playerLives = value;
//             LivesUpdateNotify?.Invoke(playerLives);
//         }
//     }
//     // private void Awake()
//     // {
//     //     if (instance != null)
//     //     {
//     //         Destroy(gameObject);
//     //     }
//     //     else
//     //     {
//     //         instance = this;
//     //         DontDestroyOnLoad(gameObject);
//     //     }
//     // }
//
//     
//     private void Start()
//     {
//         PlayerMoney = startMoney;
//         PlayerLives = startLives;
//
//         Rounds = 0;
//     }
// }
