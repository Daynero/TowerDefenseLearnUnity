using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text roundsText;
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private Button retryLevelButton;
    [SerializeField] private Button goToMenuButton;

    private void OnEnable()
    {
        roundsText.text = PlayerStats.instance.Rounds.ToString();
    }

    private void Start()
    {
        retryLevelButton.onClick.AddListener(delegate { sceneFader.FadeTo(SceneManager.GetActiveScene().name); });
        
        goToMenuButton.onClick.AddListener(delegate { sceneFader.FadeTo(menuSceneName); });
    }
}
