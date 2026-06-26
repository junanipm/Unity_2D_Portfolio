using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [Header("이동하려는 씬")]
    public string nextScene;
    int StageNum;
    public Slider portalSlider;
    public SceneMover sceneMover;

    float interTime = 0.5f;
    float currentTime;
    PlayerCombat playerCombat;
    PlayerController playerController;
    UIManager uiManager;
    public GameObject interUI;
    public bool playerOnPortal = false;
    public AudioClip audioClip;
    
    void Awake()
    {
        interUI = GameObject.Find("PortalCanvas").transform.Find("PortalInter").gameObject;
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }

    void Start()
    {
        playerController = playerCombat.gameObject.GetComponent<PlayerController>();

        portalSlider = interUI.GetComponentInChildren<Slider>();
        portalSlider.maxValue = interTime;
        portalSlider.minValue = 0;
    }

    void Update()
    {
        if (playerOnPortal)
        {
            ToNextStage();

        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            interUI.gameObject.SetActive(true);
            playerOnPortal = true;

        }


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            interUI.gameObject.SetActive(false);
            currentTime = 0;
            portalSlider.value = 0;
            playerOnPortal = false;
        }
    }
    void ToNextStage()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentTime == 0)
            {
                SoundPlay();
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            currentTime += Time.deltaTime;
            portalSlider.value = currentTime;

            if (portalSlider.value == interTime)
            {

                StartCoroutine(FadeOutBeforeSceneChange());
            }
        }
        else
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                portalSlider.value = currentTime;
            }
            if (currentTime < 0)
            {
                currentTime = 0;
            }
        }

    }
    void SoundPlay()
    {
        SoundManager.instance.SFXPlay("portal", audioClip);

    }
    IEnumerator FadeOutBeforeSceneChange()
    {
        playerController.playerPaused = true;
        uiManager.FadeOutStart();
        yield return new WaitForSeconds(uiManager.fadeDuration);
        GameManager.Instance.playerInstanceMaxHP = playerCombat.playerMaxHP;
        GameManager.Instance.playerInstanceMaxST = playerCombat.playerMaxST;
        GameManager.Instance.playerInstanceCurrentHP = playerCombat.currentHP;
        GameManager.Instance.playerInstanceCurrentST = playerCombat.currentST;

        sceneMover.SceneMove(nextScene);

    }
    
}
