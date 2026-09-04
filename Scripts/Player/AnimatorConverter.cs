using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Blue = 1,
    Yellow = 2,
    Purple = 3,
    White = 0
}

public class AnimatorConverter : MonoBehaviour
{

    [SerializeField]
    
    public PlayerState currentState{ get; private set; }
    public PlayerState rightState{ get; private set; }
    public PlayerState leftState{ get; private set; }

    


    [Header("애니메이터")]

    public RuntimeAnimatorController BlueAnim;
    public RuntimeAnimatorController YellowAnim;
    public RuntimeAnimatorController PurpleAnim;
    public RuntimeAnimatorController WhiteAnim;
    
    public Animator animator;
    public Animator changeAnimator;
    PlayerController playerController;
    PlayerCombat playerCombat;
    
    public bool converting = false;

    [Header("트레일 렌더러")]
    public Material blueMat;
    public Material purpleMat;
    public Material yellowMat;
    public Material whiteMat;
    public GameObject trailObj;
    public TrailRenderer trail;

    [Header("이미션 메테리얼")]
    public Material [] emiMat;
    SpriteRenderer spriteRenderer;

    public Material changeMaterial;

    UIManager uIManager;

    string triggerNamePerColor = "";
    void Awake()
    {
        animator = GetComponent<Animator>();
        
        trail = GetComponentInChildren<TrailRenderer>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCombat = GetComponent<PlayerCombat>();
        changeAnimator = transform.Find("Effect").GetComponent<Animator>();

        uIManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        
        
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage00" || SceneManager.GetActiveScene().name == "Stage01")
        {
            currentState = PlayerState.White;
        }
        else if (GameManager.Instance != null)
        {
            currentState = GameManager.Instance.LoadPlayerState();

            
        }

        else
        {
            Debug.LogWarning("게임매니저 안보임");

        }
        changeAnimator.gameObject.SetActive(false);



        if (currentState != PlayerState.White)
            SetupNeighbors(currentState);

        ApplyState(currentState);

        uIManager.UpdateStateIcons(currentState, leftState, rightState);

        
        animator.SetInteger("CurrentMode", (int)currentState);

    }
    void Update()
    {
        
        if (currentState != PlayerState.White && !playerCombat.isAttacking) animationChager();
        
        if(playerController.isDashing || playerCombat.isDamaged ||playerCombat.isDied)
        {
            trail.gameObject.SetActive(false);
        }
        else
            trail.gameObject.SetActive(true);
    }

    public void UnlockState(int bossIndex)
    {
        if (bossIndex == 1 && !GameManager.Instance.bossCleared01)
        {
            GameManager.Instance.bossCleared01 = true;
            
            currentState = PlayerState.Blue;
            SetupNeighbors(currentState);
            ApplyState(currentState);
            uIManager.UpdateStateIcons(currentState, leftState, rightState);
            uIManager.SetCurrentPlayerState(PlayerState.Blue);
           
            GameManager.Instance.SavePlayerState(currentState);
            animator.SetInteger("CurrentMode", 1);
        }
        else if (bossIndex == 2  && !GameManager.Instance.bossCleared02)
        {
            GameManager.Instance.bossCleared02 = true;
            SetupNeighbors(currentState);
            uIManager.UpdateStateIcons(currentState, leftState, rightState);

            
        }
        else if (bossIndex == 3 && !GameManager.Instance.bossCleared03)
        {
            GameManager.Instance.bossCleared03 = true;
            SetupNeighbors(currentState);
            uIManager.UpdateStateIcons(currentState, leftState, rightState);

            
        }
    }

    void animationChager()
    {
        if (currentState == PlayerState.White) return;
        
        if (Input.GetKeyDown(KeyCode.Q) && GameManager.Instance.bossCleared03 && !playerCombat.isCharging)
        {
            changeAnimator.gameObject.SetActive(true);
            SwapState(leftState);
            
        }
        else if (Input.GetKeyDown(KeyCode.E) && GameManager.Instance.bossCleared02&& !playerCombat.isCharging)
        {
            changeAnimator.gameObject.SetActive(true);
            SwapState(rightState);
            
        }
    }

    void SwapState(PlayerState targetState)
    {
        if (currentState == PlayerState.White || !GameManager.Instance.bossCleared02) return;
        if (converting || currentState == targetState) return;

        StartCoroutine(ConvertTime(0.5f)); 
        StartCoroutine(SpriteEnable());

        if(leftState == targetState)
        {
            PlayerState temp = currentState;
            currentState = leftState;
            uIManager.currentState = leftState;
            leftState = temp;
        }
        else if(rightState == targetState)
        {
            PlayerState temp = currentState;
            currentState = rightState;
            uIManager.currentState = rightState;
            rightState = temp;
        }
        ApplyState(currentState);
        uIManager.UpdateStateIcons(currentState, leftState, rightState);
        changeAnimator.SetTrigger(triggerNamePerColor);
        GameManager.Instance.SavePlayerState(currentState);
    }

    void SetupNeighbors(PlayerState state)
    {
       
        if (!GameManager.Instance.bossCleared02) { leftState = rightState = state; return; }

        switch (state)
        {
            case PlayerState.Blue:
                leftState = PlayerState.Purple;
                rightState = PlayerState.Yellow;
                break;
            case PlayerState.Yellow:
                leftState = PlayerState.Blue;
                rightState = PlayerState.Purple;
                break;
            case PlayerState.Purple:
                leftState = PlayerState.Yellow;
                rightState = PlayerState.Blue;
                break;
        }
    }

    void ApplyState(PlayerState state)
    {
        RuntimeAnimatorController selectedAnim = null;        
        Material trailMat = null;
        Material emissionMat = null;
        Vector3 trailPos = Vector3.zero;
        

        switch (state)
        {
            case PlayerState.Blue:
                selectedAnim = BlueAnim;
                trailMat = blueMat;
                emissionMat = emiMat[0];
                triggerNamePerColor = "ToBlue";
                ColorUtility.TryParseHtmlString("#416AF1", out Color blueColor);
                changeMaterial.SetColor("_Color", blueColor);
                trailPos = new Vector3(-0.036f, 0.291f, 0);
                break;
            case PlayerState.Yellow:
                selectedAnim = YellowAnim;
                trailMat = yellowMat;
                emissionMat = emiMat[1];
                triggerNamePerColor = "ToYellow";
                ColorUtility.TryParseHtmlString("#F1EC41", out Color yellowColor);
                changeMaterial.SetColor("_Color", yellowColor);
                trailPos = new Vector3(-0.036f, 0.309f, 0);
                break;
            case PlayerState.Purple:
                selectedAnim = PurpleAnim;
                trailMat = purpleMat;
                emissionMat = emiMat[2];
                triggerNamePerColor = "ToPurple";
                ColorUtility.TryParseHtmlString("#AA23FF", out Color purpleColor);
                changeMaterial.SetColor("_Color", purpleColor);
                trailPos = new Vector3(-0.021f, 0.189f, 0);
                break;
                
            case PlayerState.White:
                selectedAnim = WhiteAnim ?? selectedAnim;
                trailMat = whiteMat ?? trailMat;
                if (emiMat != null && emiMat.Length > 3 && emiMat[3] != null)
                    emissionMat = emiMat[3];

                triggerNamePerColor = "ToWhite";

                if (ColorUtility.TryParseHtmlString("#FFFFFF", out var whiteColor) && changeMaterial != null)
                    changeMaterial.SetColor("_Color", whiteColor);

                
                trailPos = new Vector3(-0.03f, 0.25f, 0);
                break;
        }

        animator.runtimeAnimatorController = selectedAnim;
        animator.SetInteger("CurrentMode", (int)state);
        spriteRenderer.material = emissionMat;
        trail.material = trailMat;
        trailObj.transform.localPosition = trailPos;
        
    }

    IEnumerator ConvertTime(float delay)
    {
        converting = true;
        
        yield return new WaitForSeconds(delay);
        
        converting = false;
    }
    IEnumerator SpriteEnable()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.enabled = true;
    }
}
