
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Collections;
public class NPCText : TextChanger
{
    public Canvas npcCanvas;
    public GameObject interUI;
    public UIManager uiManager;
    protected int talkInt;
    public int npcType;
    private bool playerIsInRange = false;
    public Image logoImage;
    

   
    protected override void Awake()
    {
        base.Awake();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }
    protected override void Start()
    {

        npcCanvas = GetComponentInChildren<Canvas>();
        interUI.gameObject.SetActive(false);
        npcCanvas.gameObject.SetActive(false);
        
        
    }
    protected override void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                NextTalk();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isTalking && talkInt == 0 && npcType != 1 && playerIsInRange)
        {
            interUI.gameObject.SetActive(false);
            isTalking = true;
            playerController.playerPaused = true;
            if (npcCanvas != null)
            {
                npcCanvas.gameObject.SetActive(true);

            }

            StartTalk(talkDialogue);
            playerController.PlayerPause();
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTalking && talkInt == 0)
        {
            if (npcType >= 1)
            {
                isTalking = true;
                playerController.playerPaused = true;
                if (npcCanvas != null)
                {
                    npcCanvas.gameObject.SetActive(true);

                }

                StartTalk(talkDialogue);
                playerController.PlayerPause();
            }
            else
            {
                interUI.gameObject.SetActive(true);
                playerIsInRange = true;

            }
        }

    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interUI.gameObject.SetActive(false);
            playerIsInRange = false;
        }
    }

    public override void EndTalk()
    {
        talkNum = 0;
        npcCanvas.gameObject.SetActive(false);
        playerController.PlayerResume();
        isTalking = false;
        talkInt++;
        if (npcType == 1)
        {

            Scene currentScene;
            currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            if (sceneName == "Stage02" || sceneName == "Stage05")
            {
                if (sceneName == "Stage02")
                {
                    uiManager.ToolTipUIActive();
                }


                uiManager.potionUnlock();

            }
            else if (sceneName == "Stage08")
            {
                uiManager.PotionCharging(3);
                PlayerCombat playerCombat = playerController.gameObject.GetComponent<PlayerCombat>();
                playerCombat.currentHP = GameManager.Instance.playerInstanceMaxHP;
                playerCombat.currentST = GameManager.Instance.playerInstanceMaxST;
                Debug.Log("포션");
            }




        }
        else if (npcType == 2)
        {
            uiManager.FadeOutStart();
            playerController.playerPaused = true;
            Invoke("LogoReveal", uiManager.fadeDuration + 1f);
        }
    }
    void LogoReveal()
    {
        if (logoImage != null)
        {
            StartCoroutine(MainLogoReveal());
        }
        else return;
    }
    IEnumerator MainLogoReveal()
    {
        logoImage.gameObject.SetActive(true);
        float elapsed = 0f;

        Color color = logoImage.color;
        color.a = 0f;
        logoImage.color = color;

        while (elapsed < 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / 2);

            color.a = alpha;
            logoImage.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(2f);
        GameManager.Instance.ResetGameStats();
        SceneManager.LoadScene("MainPage");
    }


}
