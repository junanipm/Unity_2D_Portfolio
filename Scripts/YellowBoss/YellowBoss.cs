<<<<<<< HEAD
 using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class YellowBoss : MonoBehaviour
{

 
    protected Animator anim;
    protected PlayerController playerController;
    protected SpriteRenderer spriteRenderer;
    [Header("카메라 관련")]
    public YellowBossTrigger yellowBossTrigger;
    public PurpleBossCombat purpleBossCombat;
    public CinemachineCamera cineCam;
    protected CinemachineConfiner2D cineCamConfiner;
    public CinemachineCamera bossCamera;
    protected YellowBossCombat yellowBossCombat;
 
    public PolygonCollider2D originalMap;
    public PolygonCollider2D bossMap;
    protected Transform cameraLock;
    protected float originalOrthoSize;
    [Header("보스 관련")]
    public float speed = 1f;
    public float distance = 1f;
    protected Vector3 startPos;
    public int bossPhase = 1;

    
 
    
 


    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        yellowBossTrigger = GameObject.Find("BossTrigger").GetComponent<YellowBossTrigger>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cineCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineCamera>();
        cineCamConfiner = GameObject.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
        yellowBossCombat = GetComponent<YellowBossCombat>();
        purpleBossCombat = GetComponent<PurpleBossCombat>();
    }
    protected virtual void Start()
    {
        originalOrthoSize = cineCam.Lens.OrthographicSize;
        startPos = transform.position;
  
        
    }

 
    protected virtual void Update()
    {
              }

    public virtual void BossStart()
    {
        anim.enabled = true;
        anim.Play("Yellow_Boss_OP");
        bossCamera.Follow = gameObject.transform;
        cineCam.Priority = 0;
        bossCamera.Priority = 1;

        
        playerController.playerPaused = true;
        Invoke("BossEntry", 5f);
        Invoke("BossIntro", 1.5f);

    }
    void BossIntro()
    {
        yellowBossCombat.BossIntro();
    }
    public virtual void BossEntry()
    {
        cineCamConfiner.BoundingShape2D = bossMap;
        playerController.PlayerResume();
        cineCam.Priority = 1;
        cineCam.Lens.OrthographicSize = 7.52f;
        bossCamera.Priority = 0;
        anim.Play("Idle");

        Invoke("BossTP", 1f);
    }
    public virtual void BossClearCamera()
    {
        anim.enabled = false;
        bossCamera.Follow = gameObject.transform;
        cineCam.Priority = 0;
        bossCamera.Priority = 1;
        playerController.playerPaused = true;
        
    }
    
    protected virtual void BossTP()
    {
        Debug.Log("1");
        yellowBossCombat.isTeleportable = true;
 
        yellowBossCombat.ThunderStart();
        yellowBossCombat.StartBossFight();
    }
}
=======
 using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class YellowBoss : MonoBehaviour
{

 
    protected Animator anim;
    protected PlayerController playerController;
    protected SpriteRenderer spriteRenderer;
    [Header("카메라 관련")]
    public YellowBossTrigger yellowBossTrigger;
    public PurpleBossCombat purpleBossCombat;
    public CinemachineCamera cineCam;
    protected CinemachineConfiner2D cineCamConfiner;
    public CinemachineCamera bossCamera;
    protected YellowBossCombat yellowBossCombat;
 
    public PolygonCollider2D originalMap;
    public PolygonCollider2D bossMap;
    protected Transform cameraLock;
    protected float originalOrthoSize;
    [Header("보스 관련")]
    public float speed = 1f;
    public float distance = 1f;
    protected Vector3 startPos;
    public int bossPhase = 1;

    
 
    
 


    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        yellowBossTrigger = GameObject.Find("BossTrigger").GetComponent<YellowBossTrigger>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cineCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineCamera>();
        cineCamConfiner = GameObject.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
        yellowBossCombat = GetComponent<YellowBossCombat>();
        purpleBossCombat = GetComponent<PurpleBossCombat>();
    }
    protected virtual void Start()
    {
        originalOrthoSize = cineCam.Lens.OrthographicSize;
        startPos = transform.position;
  
        
    }

 
    protected virtual void Update()
    {
              }

    public virtual void BossStart()
    {
        anim.enabled = true;
        anim.Play("Yellow_Boss_OP");
        bossCamera.Follow = gameObject.transform;
        cineCam.Priority = 0;
        bossCamera.Priority = 1;

        
        playerController.playerPaused = true;
        Invoke("BossEntry", 5f);
        Invoke("BossIntro", 1.5f);

    }
    void BossIntro()
    {
        yellowBossCombat.BossIntro();
    }
    public virtual void BossEntry()
    {
        cineCamConfiner.BoundingShape2D = bossMap;
        playerController.PlayerResume();
        cineCam.Priority = 1;
        cineCam.Lens.OrthographicSize = 7.52f;
        bossCamera.Priority = 0;
        anim.Play("Idle");

        Invoke("BossTP", 1f);
    }
    public virtual void BossClearCamera()
    {
        anim.enabled = false;
        bossCamera.Follow = gameObject.transform;
        cineCam.Priority = 0;
        bossCamera.Priority = 1;
        playerController.playerPaused = true;
        
    }
    
    protected virtual void BossTP()
    {
        Debug.Log("1");
        yellowBossCombat.isTeleportable = true;
 
        yellowBossCombat.ThunderStart();
        yellowBossCombat.StartBossFight();
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
