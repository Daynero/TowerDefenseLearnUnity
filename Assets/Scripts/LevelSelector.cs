using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private SceneFader fader;
    [SerializeField] private Button[] levelButtons;

    private string _levelName;
    
    private void Start()
    {
        for (int i = 0; i < levelButtons.Length - 1; i++)
        {
            _levelName = (i < 10) ? "Level0" + (i + 1) : "Level" + (i + 1);
            
            levelButtons[i].onClick.AddListener(delegate { SelectLevel(_levelName); });
        }
        
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
        }
    }

    public void SelectLevel (string levelName)
    {
        fader.FadeTo(levelName);
    }
}
