using Unity.Mathematics;
using UnityEngine;

using UnityEngine.Events;
using System.Linq;
using System.Collections;
  
using UnityEngine.UI;
public class PurpleBoss : YellowBoss
{
 
    public RuntimeAnimatorController opAnim;
    public RuntimeAnimatorController basicAnim;
    public GameObject transitionObject;
    public GameObject y_Tile;
    public GameObject y_Col;
    public GameObject p_Tile;
    public GameObject p_Col;

    public GameObject bossWall;

    public GameObject powerCrystalYellow;
    UIManager uiManager;
    
    protected AnimatorConverter animatorConverter;

    [SerializeField]
    protected Image bossInfo;

    protected YPBossSoundPlayer soundPlayer;
    protected override void Awake()
    {
        base.Awake();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        anim.runtimeAnimatorController = opAnim;
    }
    protected override void Start()
    {
        soundPlayer = GetComponent<YPBossSoundPlayer>();
        originalOrthoSize = 5.5f;
        Invoke("BossStart", 1.5f);
        bossCamera.Follow = gameObject.transform;
        cineCam.Priority = 0;
        bossCamera.Priority = 1;
        animatorConverter = GameObject.Find("Player").GetComponent<AnimatorConverter>();
        playerController.playerPaused = true;
        cineCamConfiner.BoundingShape2D = bossMap;
        uiManager.FadeInStart();
    }

    public void PlayerPause(bool pause)
    {
        playerController.playerPaused = pause;
    }
    public override void BossStart()
    {

        anim.enabled = true;
        anim.Play("Y_Boss_Die");
        cineCam.Priority = 1;
        bossCamera.Priority = 0;

        StartCoroutine(YellowUnlock());


    }

    IEnumerator YellowUnlock()
    {


        BossID currentBoss = BossID.secondBoss;


        anim.SetTrigger("Clear");
 
        powerCrystalYellow.SetActive(true);

        yield return new WaitForSeconds(7.05f);
        if (uiManager != null)
        {

            GameManager.Instance.clearedBossesCount = (int)currentBoss;
            uiManager.UpdateUIOnBossClear((int)currentBoss);

        }
        animatorConverter.UnlockState(2);

        IBossClearObserver[] observers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                                             .OfType<IBossClearObserver>()
                                             .ToArray();
        foreach (var observer in observers)
        {
            observer.OnBossClear(0);
        }
        playerController.PlayerResume();
        uiManager.ToolTipUIActive();
    }

    public void BossRevival()
    {
        playerController.playerPaused = true;
        anim.Play("P_Boss_Revival");


    }
    public void AnimatorChange()
    {
        string hexCode = "#F060FF";
        Color emiColor;
        soundPlayer.SoundPlay(2);
        GameObject transition = Instantiate(transitionObject, new Vector3(14, 0, 0), quaternion.identity);
        Destroy(transition, 1f);
        y_Col.SetActive(false);
        y_Tile.SetActive(false);
        p_Col.SetActive(true);
        p_Tile.SetActive(true);

        

        if (ColorUtility.TryParseHtmlString(hexCode, out emiColor))
        {
            spriteRenderer.material.SetColor("_Color", emiColor);
        }
        spriteRenderer.material.SetFloat("EmissionPower", 1.5f);

        anim.runtimeAnimatorController = basicAnim;
        gameObject.transform.position = new Vector3(14, 0, 0);
        BossIntro();
        Invoke("BossEntry", 3f);
    }

    public void BossIntro()
    {
        StartCoroutine(BossIntroCoroutine());
    }

    private IEnumerator BossIntroCoroutine()
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

        color.a = 1;
        yield return new WaitForSeconds(1f);

 
        StartCoroutine(BossUIFadeIn());



    }
    IEnumerator BossUIFadeIn()
    {
        float introDuration = 2f;
        float elapsed = 0f;

        Color color = bossInfo.color;
        color.a = 0f;
        bossInfo.color = color;

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

    public override void BossEntry()
    {
        playerController.PlayerResume();
        anim.Play("Idle");

        BossTP();
    }

    public void purpleBossClear()
    {
        bossWall.SetActive(false);
        cineCam.Priority = 1;
        bossCamera.Priority = 0;
        cineCam.Lens.OrthographicSize = originalOrthoSize;
        cineCam.Follow = playerController.gameObject.transform;
        cineCamConfiner.BoundingShape2D = originalMap;
        PlayerPause(false);
        purpleUnlock();
        
    }
    public void purpleUnlock()
    {
        BossID currentBoss = BossID.thirdBoss;
        if (uiManager != null)
        {

            GameManager.Instance.clearedBossesCount = (int)currentBoss;
            uiManager.UpdateUIOnBossClear((int)currentBoss);

        }
        animatorConverter.UnlockState(3);
    }
}
