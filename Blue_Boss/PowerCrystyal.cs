
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerCrystyal : MonoBehaviour
{
    public GameObject clearEffect;
    
    public Image fadeImage;

    [Header("오디오")]
    public AudioClip colorClip;    

 
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.SFXPlay("GetColor", colorClip);
            GameObject clearEf = Instantiate(clearEffect, transform.position, transform.rotation);
            Destroy(clearEf, 2f);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(FadeOut());
        }
    }
    
    protected virtual IEnumerator FadeIn()
    {
        float fadeDuration = 2f;
        float elapsed = 0f;
        Color color = fadeImage.color;
        color.a = 1;
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
    protected virtual IEnumerator FadeOut()
    {
        float fadeDuration = 0.3f;
        float elapsed = 0f;

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeIn());
    }
}
