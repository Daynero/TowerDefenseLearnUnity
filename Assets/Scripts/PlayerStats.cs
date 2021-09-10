using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int PlayerMoney;
    public int startMoney = 400;

    public static int PlayerLives;
    public int startLives = 20;

    public static int Rounds;

    private void Start()
    {
        PlayerMoney = startMoney;
        PlayerLives = startLives;

        Rounds = 0;
    }
}
