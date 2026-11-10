using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.UI;


public class FinalBoss : MonoBehaviour
{
 

    protected Animator anim;
    protected PlayerController playerController;
    protected SpriteRenderer spriteRenderer;

    public FinalBossTrigger finalBossTrigger;
    public CinemachineCamera cineCam;
    protected CinemachineConfiner2D cineCamConfiner;
    public CinemachineCamera bossCamera;
    protected FinalBossCombat finalBossCombat;

    public PolygonCollider2D originalMap;
    public PolygonCollider2D bossMap;
    protected Transform cameraLock;
    protected float originalOrthoSize;

    public GameObject animWallBefore;
    public GameObject animWallAfter;

    public GameObject bossOPParticle;

    public Image bossInfo;

    FinalBossAudio finalBossAudio;
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        finalBossTrigger = GameObject.Find("BossTrigger").GetComponent<FinalBossTrigger>();
        cineCam = GameObject.Find("Virtual Camera").GetComponent<CinemachineCamera>();
        cineCamConfiner = GameObject.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        finalBossCombat = GetComponent<FinalBossCombat>();

        finalBossAudio = GetComponent<FinalBossAudio>();
    }
    protected void Start()
    {
        originalOrthoSize = cineCam.Lens.OrthographicSize;

    }


    void Update()
    {

    }
    public void BossStart()
    {
        cineCam.Priority = 0;
        bossCamera.Priority = 1;
        StartCoroutine(BossStartCoroutine());
    }
    protected virtual IEnumerator BossStartCoroutine()
    {
        yield return new WaitForSeconds(1);

        finalBossAudio.SoundPlay(8);
        GameObject bossOP = Instantiate(bossOPParticle, new Vector3(10, -1.4f, 0), Quaternion.identity);
        Destroy(bossOP, 2f);
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.position = new Vector3(10, -3f, 0);
        anim.SetTrigger("OP");
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(BossUIFadeOut());
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("Teleport");
        playerController.playerPaused = false;
        
        yield return new WaitForSeconds(0.3f);
        gameObject.transform.position = new Vector3(10, 20f, 0);
        animWallAfter.SetActive(true);
        yield return new WaitForSeconds(0.66f);
        animWallBefore.SetActive(false);
        yield return new WaitForSeconds(1.34f);
        animWallAfter.SetActive(false);
        yield return new WaitForSeconds(1f);
        finalBossCombat.FinalBossCombatStart();
    }
    IEnumerator BossUIFadeOut()
    {
        bossInfo.gameObject.SetActive(true);
        float introDuration = 2f;
        float elapsed = 0f;

        Color color = bossInfo.color;
        color.a = 0f;

        bossInfo.color = color;

        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / introDuration);

            color.a = alpha;
            bossInfo.color = color;
            yield return null;
        }

        color.a = 1f;
        bossInfo.color = color;
        yield return new WaitForSeconds(introDuration);
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeIn()
    {

        float elapsed = 0f;
        float introDuration = 2f;
        Color color = bossInfo.color;
        color.a = 1;
        while (elapsed < introDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / introDuration);
            bossInfo.color = color;
            yield return null;
        }

        color.a = 0f;
        bossInfo.color = color;
        bossInfo.gameObject.SetActive(false);
    }
}
