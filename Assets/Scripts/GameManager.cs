using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gamePauseUI;
    [SerializeField] private string nextLevel = "Level02";
    [SerializeField] private int levelToUnlock = 2;
    [SerializeField] private SceneFader sceneFader;

    public static bool gameIsOver;

    public bool isPauseActive
    {
        private set { }
        get => ShowHideGamePause();
    }

    private void Start()
    {
        gameIsOver = false;
    }

    private void Update()
    {
        if (gameIsOver)
            return;

        if (Input.GetKeyDown("e"))
        {
            EndGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ShowHideGamePause();
        }

        if (PlayerStats.instance.PlayerLives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        Debug.Log("LEVEL WON!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        PlayerPrefs.Save();
        sceneFader.FadeTo(nextLevel);
    }
    
    public bool ShowHideGamePause()
    {
        bool isShow = !gamePauseUI.activeSelf;
        
        gamePauseUI.SetActive(isShow);
        Time.timeScale = isShow ? 0f : 1f;

        return isShow;
    }
}