using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;

    private void Update()
    {
        livesText.text = PlayerStats.PlayerLives + " LIVES";
    }
}
