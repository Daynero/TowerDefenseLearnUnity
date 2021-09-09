using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;

    // Update is called once per frame
    void Update()
    {
        livesText.text = PlayerStats.Lives + " LIVES";
    }
}
