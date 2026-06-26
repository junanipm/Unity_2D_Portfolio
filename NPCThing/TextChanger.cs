using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;


public class TextChanger : MonoBehaviour
{
    public TMP_Text dialogueText;

    public TextMeshProUGUI text;

    
    public string[] dialogues;
    public TMP_Text inter;
    public GameObject speechBubbleUI; 
    protected PlayerController playerController;
    [Header ("대사적는곳")]
    public string[] talkDialogue;

    public int talkNum;

    protected bool isTalking;
    protected virtual void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    protected virtual void Start()
    {
        playerController.playerPaused = true;
        if (speechBubbleUI != null)
            speechBubbleUI.SetActive(true);


        StartTalk(talkDialogue);
    }
    protected virtual void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                NextTalk();
            }
        }
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
        speechBubbleUI.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        inter.gameObject.SetActive(false);
        playerController.playerPaused = false;
        isTalking = false;
        
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