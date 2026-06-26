using System;
using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
public enum BossID
{
    firstBoss = 1,
    secondBoss = 2,
    thirdBoss = 3
}
public class BlueBossCombat : MonoBehaviour
{
    Animator anim;
    SpriteRenderer spriteRenderer;
    AnimatorConverter animatorConverter;
    public UIManager uiManager;
    public BlueBossLaser blueBoossLaser;
    BlueBoss blueBoss;
    public float attackCoolTime;
    [SerializeField]
    Transform target;
    float playerDistance;
    public bool isAttacking;
    public bool isAttackCool;
    bool fightBegin;
    [SerializeField]
    float bossMaxHP = 300;
    [SerializeField]
    float bossCurrentHP = 300;

    [SerializeField]
    GameObject bossPunch;
    [SerializeField]
    GameObject bossSmash;
    [SerializeField]
    GameObject bossStrike;
    [SerializeField]
    GameObject strikeReady;
    [SerializeField]
    GameObject laserRoading;

    [SerializeField]
    Vector3 punchPoint;
    [SerializeField]
    Vector3 smashPoint;
    [SerializeField]
    Vector3 strikeReadyPoint;
    [SerializeField]
    Vector3 strikePoint;
    [SerializeField]
    Vector3 laserPoint;


    [SerializeField]
    BossTrigger bossTrigger;
    public Slider bossHPSlider;
    public Vector3 firePoint;

    [SerializeField]
    GameObject powerCrystalBlue;
    [SerializeField]
    Image bossInfo;
    [SerializeField]
    GameObject[] backlight;

    [Header("오디오")]
    public AudioClip laser;
    public AudioClip op;
    public AudioClip punch;
    public AudioClip smash;
    public AudioClip wind;
    public AudioClip die;
    


    protected void Start()
    {
        bossTrigger = GameObject.Find("BossTrigger").GetComponent<BossTrigger>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        blueBoossLaser = GetComponentInChildren<BlueBossLaser>();
        blueBoss = GetComponent<BlueBoss>();
        anim = GetComponent<Animator>();

        animatorConverter = GameObject.Find("Player").GetComponent<AnimatorConverter>();
        InvokeRepeating(nameof(CheckDistance), 0f, 0.5f);
        isAttacking = false;
        isAttackCool = false;
        fightBegin = false;
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }

    void SlideActive()
    {
 
        bossHPSlider.gameObject.SetActive(true);
 
        StartCoroutine(blueBossIntro());
    }
    private IEnumerator blueBossIntro()
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
        blueBoss.PlayerPause(false);
        isAttacking = false;
        fightBegin = true;
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

 
    void Update()
    {
        if (!isAttacking && !isAttackCool && fightBegin) PatternCalculation(playerDistance);
 
 
        bossHPSlider.maxValue = bossMaxHP;
        bossHPSlider.value = bossCurrentHP;

    }
    void CheckDistance()
    {
        if (target == null) return;

        playerDistance = Vector2.Distance(transform.position, target.position);
 
    }
    void PatternCalculation(float pDiance)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (pDiance >= 12f)
        {
            FarPattern(rand);

        }
        else if (pDiance >= 3.6 && pDiance < 12)
        {
            MidPattern(rand);
        }
        else if (pDiance < 3.6)
        {
            NearPattern(rand);
        }

    }
    public void OnDamaged(float damage)
    {
        bossCurrentHP -= damage;
        if (bossCurrentHP <= 0)
        {
            BossClear();
        }
    }

    void BossClear()
    {


        anim.SetTrigger("Clear");
        bossTrigger.BossClearCam();
        bossHPSlider.gameObject.SetActive(false);
        gameObject.layer = 11;
        SoundManager.instance.SFXPlay("die", die);
        StartCoroutine(BossClearCoroutine());
        SoundManager.instance.PlayMusicByName("Stage00");

    }
    public IEnumerator BossClearCoroutine()
    {
        BossID currentBoss = BossID.firstBoss;

        blueBoss.PlayerPause(true);
        powerCrystalBlue.SetActive(true);
        yield return new WaitForSeconds(6.5f);

        IBossClearObserver[] observers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                                             .OfType<IBossClearObserver>()
                                             .ToArray();
        foreach (var observer in observers)
        {
            observer.OnBossClear(0);
        }


 

        animatorConverter.UnlockState((int)BossID.firstBoss);


        if ((int)currentBoss == 1 && uiManager != null)
        {
            uiManager.SetCurrentPlayerState(PlayerState.Blue);
            GameManager.Instance.clearedBossesCount = (int)currentBoss;
        }

 
        if (uiManager != null)
        {
            uiManager.UpdateUIOnBossClear((int)currentBoss);
        }
        blueBoss.PlayerPause(false);
        backlight[0].gameObject.SetActive(true);
        backlight[1].gameObject.SetActive(true);

        uiManager.ToolTipUIActive();
    }

    void testSecondBoss()
    {
        BossID currentBoss = BossID.secondBoss;

        if (uiManager != null)
        {
            GameManager.Instance.clearedBossesCount = (int)currentBoss;
            uiManager.UpdateUIOnBossClear((int)currentBoss);

        }
        animatorConverter.UnlockState((int)currentBoss);
        Invoke("testThirdBoss", 0.1f);
    }
    void testThirdBoss()
    {
        BossID currentBoss = BossID.thirdBoss;

        if (uiManager != null)
        {
            GameManager.Instance.clearedBossesCount = (int)currentBoss;
            uiManager.UpdateUIOnBossClear((int)currentBoss);

        }
        animatorConverter.UnlockState((int)currentBoss);
    }
    void FarPattern(int rand)
    {
        if (rand < 80) 
        {
            anim.SetTrigger("Strike");
            anim.SetInteger("Pattern", 3);

        }
        else 
        {
            anim.SetTrigger("Laser");
            anim.SetInteger("Pattern", 2);
        }
        StartCoroutine(AttackCoolDown());
    }
    void MidPattern(int rand)
    {
        if (rand < 35) 
        {
            anim.SetTrigger("Smash");
            anim.SetInteger("Pattern", 1);
        }
        else if (rand < 70) 
        {
            anim.SetTrigger("Strike");
            anim.SetInteger("Pattern", 3);
        }
        else 
        {
            anim.SetTrigger("Laser");
            anim.SetInteger("Pattern", 2);
        }
        StartCoroutine(AttackCoolDown());
        Debug.Log("중간거");
    }
    void NearPattern(int rand)
    {
        if (rand < 40) 
        {
            anim.SetTrigger("Punch");
            anim.SetInteger("Pattern", 0);
        }
        else if (rand < 70) 
        {
            anim.SetTrigger("Smash");
            anim.SetInteger("Pattern", 1);
        }
        else 
        {
            anim.SetTrigger("Strike");
            anim.SetInteger("Pattern", 3);
        }
        StartCoroutine(AttackCoolDown());
       
    }

    void AttackingCheck(int value)
    {
        isAttacking = value == 0 ? true : false;
    }

    
    void BlueBossLaser()
    {
        GameObject obj = Instantiate(laserRoading, laserPoint, Quaternion.identity);
        Destroy(obj, 1f);
    }
    void BlueBossSmash()
    {
        Instantiate(bossSmash, smashPoint, Quaternion.identity);
    }
    void BlueStrikeReady()
    {

        GameObject obj = Instantiate(strikeReady, strikeReadyPoint, Quaternion.identity);
        Destroy(obj, 0.6f);
    }
    void BlueBossStrike()
    {

        Instantiate(bossStrike, strikePoint, Quaternion.identity);

    }
    IEnumerator AttackCoolDown()
    {
        isAttackCool = true;
        yield return new WaitForSeconds(attackCoolTime);
        isAttackCool = false;
    }

    void ShootLaser()
    {

        Vector3 startPoint = firePoint;

        float targetX = startPoint.x - 10;

        blueBoossLaser.FireLaser(startPoint, targetX, 0.4f);
    }
    void SoundPlayLaser(int sound)
    {


        if (sound == 0)
        {
            SoundManager.instance.SFXPlay("laser", laser);
        }
        else if (sound == 1)
        {
            SoundManager.instance.SFXPlay("op", op);
        }
        else if (sound == 2)
        {
            SoundManager.instance.SFXPlay("punch", punch);
        }
        else if (sound == 3)
        {
            SoundManager.instance.SFXPlay("smash", smash);
        }
        else
        {
            SoundManager.instance.SFXPlay("wind", wind);
        }

        
    }
    
}
