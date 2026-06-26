using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    UIManager uiManager;
    public bool bossCleared01;
    public bool bossCleared02;
    public bool bossCleared03;
    public int clearedBossesCount;
    public bool isPaused;
    public bool isToolTipOn;
    public GameObject menuUI;

    public PlayerState savedState = PlayerState.Blue; 

    public int currentPotionMax;
    public int currentPotion;

    public float playerInstanceMaxHP;
    public float playerInstanceMaxST;
    public float playerInstanceCurrentHP;
    public float playerInstanceCurrentST;
    Scene currentScene;
    public float fadeDuration;
    public Image fadeImage;
    public GameObject manualSet;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        savedState = PlayerState.Blue;

        playerInstanceCurrentHP = 100;
        playerInstanceCurrentST = 100;
        menuUI = GameObject.Find("Settings");
        Transform manualSetTransform = menuUI.transform.Find("ManualSet");
        manualSet = manualSetTransform.gameObject;
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();

        currentScene = SceneManager.GetActiveScene();
        fadeDuration = uiManager.fadeDuration;
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 120;
        ResetGameStats();
    }
    

    void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;



        PlayerCombat playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();

        if (playerCombat != null && playerCombat.currentHP > 0)
        {
            playerInstanceCurrentHP = playerCombat.currentHP;
            playerInstanceCurrentST = playerCombat.currentST;
            playerInstanceMaxHP = playerCombat.playerMaxHP;
            playerInstanceMaxST = playerCombat.playerMaxST;
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isToolTipOn)
        {

            if (isPaused)
            {
                manualSet.gameObject.SetActive(false);
                ResumeGame();
            }
            else
            {
                PauseGame();
 
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        

        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        PlayerCombat playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();

        menuUI = GameObject.Find("Settings");
        Transform manualSetTransform = menuUI.transform.Find("ManualSet");
        manualSet = manualSetTransform.gameObject;

        if (menuUI != null && !isPaused)
        {
            menuUI.gameObject.SetActive(false);
        }
        
        if (manualSet != null & !isPaused)
        {
            manualSet.gameObject.SetActive(false);
        }

        playerCombat.playerMaxHP = playerInstanceMaxHP;
        playerCombat.playerMaxST = playerInstanceMaxST;

        playerCombat.currentST = playerInstanceCurrentST;
        playerCombat.currentHP = playerInstanceCurrentHP;

        if (fadeImage == null)
        {
            fadeImage = GameObject.Find("Fade").GetComponent<Image>();

            if (fadeImage == null)
            {
                Debug.LogError("FadePanel을 찾을 수 없습니다!");

            }
        }

        StartCoroutine(FadeIn());
    }
    public void ResumeGame()
    {

        menuUI.SetActive(false); 
        
        Time.timeScale = 1f;          
        isPaused = false;

        if (manualSet.activeInHierarchy)
        {
            manualSet.gameObject.SetActive(false);
        }
            Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void PauseGame()
    {

        menuUI.SetActive(true);  
        Time.timeScale = 0f;          
        isPaused = true;
        
   
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void SavePlayerState(PlayerState state)
    {
        savedState = state;
    }

    public PlayerState LoadPlayerState()
    {
        return savedState;
    }

    private IEnumerator FadeIn()
    {
        fadeDuration = 1.5f;
        Color fadeColor = fadeImage.color;
        fadeColor.a = 1f;
        fadeImage.color = fadeColor;
        fadeImage.gameObject.SetActive(true);
        fadeColor.a = 1f;

        float elapsedTime = 0f;


        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = 1.0f - (elapsedTime / fadeDuration); 
            fadeColor.a = newAlpha;
            fadeImage.color = fadeColor;
            yield return null; 
        }


        fadeColor.a = 0f;
        fadeImage.color = fadeColor;
    }
    
    public void ResetGameStats()
    {
        Debug.Log("GameManager 데이터를 초기화합니다.");


        savedState = PlayerState.White; 
        playerInstanceCurrentHP = 100;
        playerInstanceCurrentST = 100;
        playerInstanceMaxHP = 100; 
        playerInstanceMaxST = 100;

        currentPotionMax = 0; 
        currentPotion = 0;

        bossCleared01 = false;
        bossCleared02 = false;
        bossCleared03 = false;
        clearedBossesCount = 0;

    }
}
