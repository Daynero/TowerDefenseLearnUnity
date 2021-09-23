using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelToLoad = "MainLevel";
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(delegate { sceneFader.FadeTo(levelToLoad); });
        
        quitButton.onClick.AddListener(delegate
        {
            Debug.Log("Exiting...");
            Application.Quit();
        });
    }
}
