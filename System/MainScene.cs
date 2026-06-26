using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public bool isFadeIn;
    public bool isFadeOut;
    public Image fadeImage;
    public float fadeDuration;

    void Start()
    {
        StartCoroutine(FadeIn());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameStart()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(StartWithFadeOut());
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public IEnumerator StartWithFadeOut()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while(elapsed < fadeDuration)
        {
            elapsed+= Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed/fadeDuration);
            fadeImage.color = color;
            
            yield return null;
        }
        SceneManager.LoadScene("Stage00");
        color.a = 1f;
        fadeImage.color = color;
    }

    public IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }
}
