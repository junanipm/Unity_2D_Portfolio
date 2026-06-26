using UnityEngine;

public class BlueBoss : MonoBehaviour
{
    

    Animator anim;
    PlayerController playerController;
    public BossTrigger bossTrigger;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private Material bossMat;
    [SerializeField]
    private Material bossOPMat;
    [SerializeField]
    private GameObject startEffect;
    public AudioClip change;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bossTrigger = GameObject.Find("BossTrigger").GetComponent<BossTrigger>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        anim.enabled = false;
    }

    public void BossStart()
    {
        anim.enabled = true;
        anim.Play("Blue_Boss_OP");
    
        playerController.playerPaused = true;

    }
    void FirstShake()
    {
        bossTrigger.ShakeCamera(0.5f, 0.1f);
    }
    void SecondShake()
    {
        bossTrigger.ShakeCamera(1.5f, 0.25f);
    }
    void LastShake()
    {
        bossTrigger.ShakeCamera(5f, 0.25f);
    }
    void StopShake()
    {
        bossTrigger.StopShake();

    }
    void SmoothStop()
    {
        bossTrigger.SmoothStopShake(2f);
        
        anim.Play("Idle");
        spriteRenderer.material = bossMat;

    }
    void MatChange()
    {
        startEffect.SetActive(true);
        SoundManager.instance.SFXPlay("ch", change);
    }
    void BossClearMatchange()
    {
        spriteRenderer.material = bossOPMat;
    }

    public void PlayerPause(bool pause)
    {
        playerController.playerPaused = pause;
    }
    
}
