using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class PowerCrystalPurple : PowerCrystyal
{
    public Transform playerLocation;
    public float speed = 3;
    public  GameObject[] backGround;
    PurpleBoss purpleBoss;
    Image targetImage;
    bool isStoped;
    public AudioClip audioClip;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        GameObject fadeImage = GameObject.Find("PurpleImage");
        if (player != null)
        {
            playerLocation = player.transform;
        }
        targetImage = fadeImage.GetComponent<Image>();

        purpleBoss = GameObject.Find("PurpleBoss").GetComponent<PurpleBoss>();
        isStoped = true;


        Invoke("Stop", 1.1f);
        backGround[0] = GameObject.Find("3_BackGround_M");
        backGround[1] = GameObject.Find("3_BackGround_R");
        backGround[2] = GameObject.Find("3_BackGround_L");

    }
        
    void Update()
    {
        if (playerLocation != null && !isStoped)
        {

            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, playerLocation.position, step);
        }
    }
    void Stop()
    {
        isStoped = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject clearEf = Instantiate(clearEffect, transform.position, transform.rotation);
            SoundManager.instance.SFXPlay("clear", audioClip);
            Destroy(clearEf, 2f);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(FadeOut());
        }
    }


    void backgroundChangeAndClear()
    {
        backGround[0].SetActive(true);
        backGround[1].SetActive(true);
        backGround[2].SetActive(true);
        IBossClearObserver[] observers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                                             .OfType<IBossClearObserver>()
                                             .ToArray();
        foreach (var observer in observers)
        {
            observer.OnBossClear(1);
        }
        purpleBoss.purpleBossClear();

    }
    
    protected override IEnumerator FadeIn()
    {
        float fadeDuration = 2f;
        float elapsed = 0f;
        Color color = targetImage.color;
        color.a = 1;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            targetImage.color = color;
            yield return null;
        }

        color.a = 0f;
        targetImage.color = color;
        backgroundChangeAndClear();
        targetImage.gameObject.SetActive(false);
    }
    protected override IEnumerator FadeOut()
    {
        float fadeDuration = 0.3f;
        float elapsed = 0f;

        Color color = targetImage.color;
        color.a = 0f;
        targetImage.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            color.a = alpha;
            targetImage.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeIn());
    }
}
