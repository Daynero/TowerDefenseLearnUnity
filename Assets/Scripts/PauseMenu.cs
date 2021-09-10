using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private Button toggleGamePauseButton;
    [SerializeField] private Button retryLevelButton;
    [SerializeField] private Button goToMenuButton;

    private void Start()
    {
        toggleGamePauseButton.onClick.AddListener(delegate { ToggleGamePause(); });

        retryLevelButton.onClick.AddListener(delegate
        {
            ToggleGamePause();
            sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        });

        goToMenuButton.onClick.AddListener(delegate
        {
            ToggleGamePause();
            sceneFader.FadeTo(menuSceneName);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ToggleGamePause();
        }
    }

    private void ToggleGamePause()
    {
        ui.SetActive(!ui.activeSelf);

        Time.timeScale = ui.activeSelf ? 0f : 1f;
    }
}