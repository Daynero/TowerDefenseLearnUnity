using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private AnimationCurve curve;
    
    private static SceneFader _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    private IEnumerator FadeIn()
    {
        float time = 1f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            float alpha = curve.Evaluate(time);
            img.color = new Color (0f, 0f, 0f, alpha);
            yield return 0;
        }
    }

    private IEnumerator FadeOut(string scene)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime;
            float alpha = curve.Evaluate(time);
            img.color = new Color(0f, 0f, 0f, alpha);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
        StartCoroutine(FadeIn());
    }
}
