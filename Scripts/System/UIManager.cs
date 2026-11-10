using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    public Transform canvasTransform;
    PlayerCombat playerCombat;
    public Slider hpSlider;
    public Slider stSlider;
    public Image stFillImage;
    public Image cooldownImage;
    public SkillCoolManager skillCoolManager;
    public PlayerState currentState;

    public bool isFadeIn;
    public bool isFadeOut;
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    [Header("모드 아이콘")]
    public Image currentIcon;
    public Image leftIcon;
    public Image rightIcon;
    public Image skillIcon;
    public Image stateFrame;
    public Image skillFrame;

    public Sprite blueIcon;
    public Sprite yellowIcon;
    public Sprite purpleIcon;
    public Sprite whiteIcon;
    public Sprite lockedIcon;

    public Sprite blueStateFrame;
    public Sprite blueSkillFrame;
    public Sprite blueSkillIcon;
    public Sprite blueStemina;
    public Sprite yellowStateFrame;
    public Sprite yellowSkillFrame;
    public Sprite yellowStemina;
    public Sprite yellowSkillIcon;
    public Sprite purpleStateFrame;
    public Sprite purpleSkillFrame;
    public Sprite purpleSkillIcon;
    public Sprite purpleStemina;

    public Sprite whiteStemina;

    [Header("포션")]
    public int potionCurrent;
    public int potionMax;
    public Image[] potionSlot;
    public Sprite potionEmpty;
    public Sprite potionFull;
    [Header("툴팁")]
    public Image toolTipUI;
    bool toolTipOn;
    [Header("오디오")]
    public AudioClip healClip;
    public Image[] decoImage;

    void Awake()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        skillCoolManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillCoolManager>();
    }
    void Start()
    {
        if (currentState == PlayerState.White)
            SetInitialLockState();

        HPUIUpdate();
        STUIUpdate();

        if (GameManager.Instance.currentPotionMax == 0)
        {
            SetPotionMax(0);

        }
        else
        {
            SetPotionMax(GameManager.Instance.currentPotionMax);
            SetCurrentPotion(GameManager.Instance.currentPotion);

        }
        UpdatePotionUI();
  
        DecoUIActivate();
    }

    void Update()
    {
        float progress = skillCoolManager.GetCooldownProgress(currentState);
        cooldownImage.fillAmount = progress;

        HPUIUpdate();
        STUIUpdate();


        if (Input.GetKeyDown(KeyCode.R) && playerCombat.currentHP < playerCombat.playerMaxHP)
        {
            UsePotion();


        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            PotionUpgrade(3);

        }

        if (toolTipUI != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Color color = toolTipUI.color;
                color.a = 0;
                toolTipUI.color = color;
                toolTipUI.gameObject.SetActive(false);
                Time.timeScale = 1f;
                GameManager.Instance.isPaused = false;
                GameManager.Instance.isToolTipOn = false;
            }

        }
        testhamsu();
    }
    public void SetInitialLockState()
    {
        if (leftIcon != null) leftIcon.sprite = lockedIcon;
        if (rightIcon != null) rightIcon.sprite = lockedIcon;
        if (skillIcon != null)
        {
            skillIcon.sprite = lockedIcon;
            RectTransform rect = skillIcon.GetComponent<RectTransform>();

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 41f);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 51f);

        }

    }
    public void SetCurrentPlayerState(PlayerState state)
    {
        currentState = state;
    }
    public void UpdateUIOnBossClear(int bossesCleared)
    {
        GameManager.Instance.clearedBossesCount = bossesCleared;

        switch (GameManager.Instance.clearedBossesCount)
        {
            case 1:
                
                if (currentIcon != null)
                {
                    currentIcon.sprite = blueIcon;
                    stateFrame.sprite = blueStateFrame;
                    skillFrame.sprite = blueSkillFrame;
                    skillIcon.sprite = blueSkillIcon;
                    RectTransform rect = skillIcon.GetComponent<RectTransform>();

                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70f);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 58f);
                    playerCombat.playerMaxHP = 200;
                    playerCombat.currentHP = 200;
                    playerCombat.currentST = 150;
                    playerCombat.playerMaxST = 150;
                    playerCombat.attackPower = 20;
                    GameManager.Instance.playerInstanceMaxHP = playerCombat.playerMaxHP;
                    GameManager.Instance.playerInstanceMaxST = playerCombat.playerMaxST;

                    DecoUIActivate();
                }
                
                break;
            case 2:
                

                if (rightIcon != null)
                {
                    rightIcon.sprite = yellowIcon;
                    playerCombat.playerMaxHP = 300;
                    playerCombat.currentHP = 300;
                    playerCombat.playerMaxST = 200;
                    playerCombat.attackPower = 25;

                    DecoUIActivate();
                    GameManager.Instance.playerInstanceMaxHP = playerCombat.playerMaxHP;
                    GameManager.Instance.playerInstanceMaxST = playerCombat.playerMaxST;
                }
             
                break;
            case 3:
            
                if (leftIcon != null)
                {
                    playerCombat.currentHP = playerCombat.playerMaxHP;
                    playerCombat.currentST = playerCombat.playerMaxST;
                    leftIcon.sprite = purpleIcon; 
                    playerCombat.attackPower = 30;
                }
                break;
        }
    }

    void HPUIUpdate()
    {
        RectTransform hpRectTran = hpSlider.GetComponent<RectTransform>();

        hpRectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3f * playerCombat.playerMaxHP);
        hpSlider.maxValue = playerCombat.playerMaxHP;
        hpSlider.value = playerCombat.currentHP;
    }

    void STUIUpdate()
    {
        RectTransform stRectTran = stSlider.GetComponent<RectTransform>();

        stRectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2f * playerCombat.playerMaxST);
        stSlider.maxValue = playerCombat.playerMaxST;
        stSlider.value = playerCombat.currentST;
    }
    public void FadeInStart()
    {

        StartCoroutine(FadeIn());
    }
    public void FadeOutStart()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }


    public IEnumerator FadeIn()
    {

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
    private IEnumerator FadeOut()
    {

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

 
        Debug.Log("화면 완전 어두워짐");
    }

    public void UpdateStateIcons(PlayerState current, PlayerState left, PlayerState right)
    {

        Debug.Log(GameManager.Instance.clearedBossesCount);

        SetIcon(currentIcon, current, new Vector2(-818.7f, 447.2f), new Vector2(100f, 100f), false);

        if (GameManager.Instance.clearedBossesCount < 2)
        {
            SetIcon(rightIcon, right, new Vector2(-746f, 396f), new Vector2(41f, 51f), true);

        }
        else
        {
            SetIcon(rightIcon, right, new Vector2(-746f, 396f), new Vector2(80f, 80f), false);
            SetFrameAndSkill(current, false);
        }


        if (GameManager.Instance.clearedBossesCount < 3)
        {
            SetIcon(leftIcon, left, new Vector2(-889f, 396f), new Vector2(41f, 51f), true);
            SetFrameAndSkill(current, false);
        }
        else
        {
            SetIcon(leftIcon, left, new Vector2(-889f, 396f), new Vector2(80f, 80f), false);
            SetFrameAndSkill(current, false);
        }
    }

    void SetIcon(Image img, PlayerState state, Vector2 anchoredPos, Vector2 size, bool isLocked)
    {
        
        if (isLocked)
        {
            img.sprite = lockedIcon;
            RectTransform rect = img.GetComponent<RectTransform>();
            rect.anchoredPosition = anchoredPos;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 41f);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 51f);
        }
        else
        {

            switch (state)
            {
                case PlayerState.Blue:
                    img.sprite = blueIcon;
                    SetFrameAndSkill(PlayerState.Blue, false);
                    

                    break;
                case PlayerState.Yellow:
                    img.sprite = yellowIcon;

                   
                    break;
                case PlayerState.Purple:
                    img.sprite = purpleIcon;
                   
                    break;
                case PlayerState.White:
                    img.sprite = whiteIcon;
                    break;
            }
            RectTransform rect = img.GetComponent<RectTransform>();
            rect.anchoredPosition = anchoredPos;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
    void SetFrameAndSkill(PlayerState state, bool isLocked)
    {
        if (isLocked) return;
        else
        {
            switch (state)
            {
                case PlayerState.Blue:
                    stateFrame.sprite = blueStateFrame;
                    skillFrame.sprite = blueSkillFrame;
                    stFillImage.sprite = blueStemina;
                    skillIcon.sprite = blueSkillIcon;
                    RectTransform b_rect = skillIcon.GetComponent<RectTransform>();

                    b_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 70f);
                    b_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 58f);
                    break;
                case PlayerState.Yellow:
                    stateFrame.sprite = yellowStateFrame;
                    skillFrame.sprite = yellowSkillFrame;
                    stFillImage.sprite = yellowStemina;
                    skillIcon.sprite = yellowSkillIcon;
                    RectTransform y_rect = skillIcon.GetComponent<RectTransform>();

                    y_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 62f);
                    y_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 62f);
                    break;
                case PlayerState.Purple:

                    stateFrame.sprite = purpleStateFrame;
                    skillFrame.sprite = purpleSkillFrame;
                    stFillImage.sprite = purpleStemina;
                    skillIcon.sprite = purpleSkillIcon;
                    RectTransform p_rect = skillIcon.GetComponent<RectTransform>();

                    p_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 63f);
                    p_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 64f);
                    break;
            }
        }
    }
    public void SetPotionMax(int newMax)
    {
        potionMax = newMax;
        GameManager.Instance.currentPotionMax = potionMax;
        UpdatePotionUI();
    }
    public void SetCurrentPotion(int current)
    {
        potionCurrent = current;
    }

    public void PotionCharging(int newMax)
    {
        potionCurrent = newMax;
        GameManager.Instance.currentPotion = potionCurrent;
        UpdatePotionUI();
    }


    void UpdatePotionUI()
    {
        for (int i = 0; i < potionSlot.Length; i++)
        {
            if (i < potionMax)
            {
                potionSlot[i].gameObject.SetActive(true); 


                if (i < potionCurrent)
                {
                    potionSlot[i].sprite = potionFull;
                }
                else
                {
                    potionSlot[i].sprite = potionEmpty;
                }
            }
            else
            {

                potionSlot[i].gameObject.SetActive(false);
            }
        }

    }
    public void potionUnlock()
    {
        if (potionMax == 0)
        {
            potionMax = 1;
            SetPotionMax(1); 
            GameManager.Instance.currentPotion = 1;
            SetCurrentPotion(1);
            UpdatePotionUI();
        }
        else if (potionMax == 1)
        {
            potionMax = 3;
            SetPotionMax(3);
            GameManager.Instance.currentPotion = 3;
            SetCurrentPotion(3);
            UpdatePotionUI();
        }
        else
        {

        }

    }

    public void UsePotion()
    {

        if (potionCurrent <= 0 || playerCombat.currentHP <= 0)
        {
            Debug.Log("포션이 없습니다.");
            return;
        }

        SoundManager.instance.SFXPlay("Heal", healClip);

        Debug.Log("포션을 사용했습니다.");
        playerCombat.healEffectActive();

        playerCombat.currentHP = Mathf.Min(playerCombat.currentHP + 40, playerCombat.playerMaxHP);
        
        potionCurrent--;
        GameManager.Instance.currentPotion = potionCurrent;

       
        if (potionMax == 1)
        {
           
            potionSlot[0].sprite = potionEmpty;
        }
        else if (potionMax == 3)
        {
            
            potionSlot[potionCurrent].sprite = potionEmpty;
        }
    }

    public void PotionUpgrade(int max)

    {

        potionMax = max;
        potionUnlock();
        SetCurrentPotion(potionMax);
        UpdatePotionUI();
    }

    public void ToolTipUIActive()
    {
        Debug.Log($"[ToolTip] 1. ToolTipUIActive() 호출됨. toolTipOn={toolTipOn}, toolTipUI={toolTipUI != null}");
        if (!toolTipOn && toolTipUI != null)
        {
            toolTipUI.gameObject.SetActive(true);

            Time.timeScale = 0f;         
            GameManager.Instance.isPaused = true;
            GameManager.Instance.isToolTipOn = true;
            StartCoroutine(ToolTipUICoroutine());
        }
        else
        {
            Debug.LogWarning("[ToolTip] 2. 조건 실패로 코루틴 실행 안 함!");
        }


    }

    IEnumerator ToolTipUICoroutine()
    {
        float elapsed = 0;
        float duration = 0.5f;

        Color og = toolTipUI.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float newA = Mathf.Lerp(0, 1, elapsed / duration);
            toolTipUI.color = new Color(og.r, og.g, og.b, newA);
            yield return null;
        }
        toolTipOn = true;
    }
    bool testbool = false;

    void testhamsu()
    {
        if (Input.GetKeyDown(KeyCode.F3) && Input.GetKeyDown(KeyCode.F4) && !testbool)
        {
            AnimatorConverter animatorConverter = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorConverter>();

            animatorConverter.UnlockState(1);
            SetCurrentPlayerState(PlayerState.Blue);

            GameManager.Instance.clearedBossesCount = 1;
            UpdateUIOnBossClear(1);
            animatorConverter.UnlockState(2);
            GameManager.Instance.clearedBossesCount = 2;
            UpdateUIOnBossClear(2);
            animatorConverter.UnlockState(3);
            GameManager.Instance.clearedBossesCount = 3;
            UpdateUIOnBossClear(3);
            testbool = true;
        }
    }
    public void DecoUIActivate()
    {
        int decoint = 0;
        if (GameManager.Instance.clearedBossesCount == 0)
        {
            decoint = 0;
        }
        else if (GameManager.Instance.clearedBossesCount == 1)
        {
            decoint = 2;
        }
        else if (GameManager.Instance.clearedBossesCount == 2)
        {
            decoint = 5;
        }
        else
        {
            decoint = 5;
        }

        for (int i = 0; i < decoint; i++)
            {
                decoImage[i].gameObject.SetActive(true);
            }
    }
}
