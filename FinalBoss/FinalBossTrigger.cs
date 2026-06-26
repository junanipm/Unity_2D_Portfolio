using UnityEngine;
using Unity.Cinemachine;
using Unity.Mathematics;
using System.Collections;

using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;

public class FinalBossTrigger : MonoBehaviour
{
    public FinalBoss finalBoss;
    protected PlayerCombat playerCombat;
    protected PlayerController playerController;
    [SerializeField]
    public GameObject bossWall;
 
    public Animator wallAnim;
    public Animator pillarAnim;


    protected bool hasPaused;
    
    SpriteRenderer wallSpriteRenderer;
 

    public CanvasGroup fadeGroup;
    public RectTransform topImage;
    public RectTransform bottomImage;
    public float openDuration = 1;

    public float fadeDuration = 1f;
    public AudioClip mapbreakdown;

    [Header("보스 다이얼로그")]
    public TMP_Text dialogueText;
    public TextMeshProUGUI text;
    public string[] dialogues;
    protected bool isTalking;
    public TMP_Text inter;
    public int talkNum;
    [Header("대사적는곳")]
    public string[] talkDialogue;

    public float backgroundDelay = 3f;
    protected virtual void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    protected virtual void Start()
    {

        finalBoss = GameObject.Find("FinalBoss").GetComponent<FinalBoss>();
        
        playerController.playerPaused = true;
        isTalking = true;
        StartTalk(talkDialogue);
    }

    protected virtual void Update()
    {
        PlayerPositionFounder();

        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                NextTalk();
            }
        }
    }
    protected virtual void PlayerPositionFounder()
    {
        if (playerController.gameObject.transform.position.x >= -7.5f && !hasPaused)
        {
            hasPaused = true;

            playerController.playerPaused = true;
            playerController.PlayerIdlePlay();
            finalBoss.BossStart();
            bossWall.SetActive(true);

            Invoke("BackgroundChanger", backgroundDelay);

        }
    }
    void BackgroundChanger()
    {
        SoundManager.instance.SFXPlay("mapbreakdown", mapbreakdown);
        wallAnim.enabled = true;
        pillarAnim.enabled = true;
    }

    protected void StartTalk(string[] talks)
    {
        dialogues = talks;

        StartCoroutine(Typing(dialogues[talkNum]));
        isTalking = true;
    }

    protected void NextTalk()
    {
        dialogueText.text = null;

        talkNum++;
        if (talkNum == dialogues.Length)
        {
            EndTalk();
            return;
        }
        StartCoroutine(Typing(dialogues[talkNum]));
    }

    public virtual void EndTalk()
    {
        talkNum = 0;

        
        
        isTalking = false;
        StartCoroutine(EndTalkCoroutine());
    }
    IEnumerator EndTalkCoroutine()
    {
        if (fadeGroup == null)
        {
            Debug.LogError("CanvasGroup이 연결되지 않았습니다!");
            yield break; 
        }


        fadeGroup.alpha = 1f;


        float elapsedTime = 0f;


        float effectiveFadeDuration = fadeDuration;
        if (effectiveFadeDuration <= 0)
        {
            effectiveFadeDuration = 1.0f; 
        }

        while (elapsedTime < effectiveFadeDuration)
        {
            
            elapsedTime += Time.unscaledDeltaTime;

           
            fadeGroup.alpha = 1.0f - (elapsedTime / effectiveFadeDuration);

            yield return null; 
        }


        fadeGroup.alpha = 0f;

        dialogueText.gameObject.SetActive(false);
        inter.gameObject.SetActive(false);
        fadeGroup.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(UIOpen());
    }

    IEnumerator UIOpen()
    {
        float elapsed = 0;
        float startY_Top = 0;
        float startY_Bot = 0;

        float targetY_Top = topImage.rect.height;
        float targetY_Bot = -bottomImage.rect.height;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / openDuration;
            t = t * t * (3f - 2f * t);

            float newY_Top = Mathf.Lerp(startY_Top, targetY_Top, t);
            float newY_Bot = Mathf.Lerp(startY_Bot, targetY_Bot, t);

            topImage.anchoredPosition = new Vector2(topImage.anchoredPosition.x, newY_Top);
            bottomImage.anchoredPosition = new Vector2(bottomImage.anchoredPosition.x, newY_Bot);
            yield return null;
        }

        topImage.anchoredPosition = new Vector2(topImage.anchoredPosition.x, targetY_Top);
        bottomImage.anchoredPosition = new Vector2(bottomImage.anchoredPosition.x, targetY_Bot);

        topImage.gameObject.SetActive(false);
        bottomImage.gameObject.SetActive(false);
        playerController.playerPaused = false;
    }

    public static void TMPDOText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);

    }
    protected IEnumerator Typing(string talk)
    {


        if (talk.Contains("  ")) talk = talk.Replace("  ", "\n");
        text.text = talk;
        TMPDOText(text, 1);

        yield return new WaitForSeconds(1.5f);
        
    }
    

}
