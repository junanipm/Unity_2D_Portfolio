
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using Unity.Collections;
  
using Unity.VisualScripting;

  
using UnityEngine;
using UnityEngine.UI;
public class YellowBossCombat : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected AnimatorConverter animatorConverter;
    protected UIManager uiManager;
    protected BoxCollider2D boxCollider;
    protected YellowBoss yellowBoss;
    public float attackCoolTime = 3f;
    public float teleportCoolTime = 8f;

    protected Coroutine patternCoroutine;

    [SerializeField]
    protected Transform target;
    protected Vector3 playerPosition;
    float playerDistance;
    public bool isAttacking;
    public bool isAttackCool;
    public bool isGroggy;
    public bool isTeleportable;

    public float thunderMinDelay = 10f;
    public float thunderMaxDelay = 15f;

    [SerializeField]
    protected float bossMaxHP = 500;
    [SerializeField]
    protected float bossCurrentHP = 500;
    [SerializeField]
    protected GameObject bossTeleportEffect;
    [SerializeField]
    protected List<GameObject> bossThunder;
    [SerializeField]
    protected GameObject bossMeteor;
    [SerializeField]
    protected GameObject bossSphere;
    [SerializeField]
    protected List<GameObject> PatternPrecursor;
    [SerializeField]
    protected GameObject bossSpine;
    [SerializeField]
    protected List<Vector3> spineSpawnPoint;
    [SerializeField]
    protected  GameObject[] bossCrystal;
    protected readonly float[] spineAngles = new float[] { 72f, 36f, 12f, -12f, -36f, -72f };
    protected List<Yellow_Boss_Spine> activeSpines = new List<Yellow_Boss_Spine>();

    protected int bossLocation = 1; 
    
    public int crystalLife = 3;
    public Slider bossHPSlider;
    protected Vector3 meteorSpawnPosition = new Vector3(0,0,0);
    protected Vector3 spherepawnPosition = new Vector3(0,0,0);

    [SerializeField]
    Image bossInfo;

    protected YPBossSoundPlayer soundPlayer;


 
    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }
    protected virtual void Start()
    {
        soundPlayer = GetComponent<YPBossSoundPlayer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        animatorConverter = GameObject.Find("Player").GetComponent<AnimatorConverter>();

        isAttacking = false;
        isAttackCool = false;
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        InvokeRepeating("UpdatePosition", 0.5f, 1f);

        isAttacking = false;
        isAttackCool = false;
        isTeleportable = false;
        meteorSpawnPosition = new Vector3(23f, 10f, 1f);
        yellowBoss = GetComponent<YellowBoss>();
 

    }

 
    protected virtual void Update()
    {
        if (crystalLife == 0 && !isGroggy)
        {
            StartCoroutine(GroggyCoroutine());
        }
        bossHPSlider.maxValue = bossMaxHP;
        bossHPSlider.value = bossCurrentHP;

 
    }

    public virtual void StartBossFight()
    {
        if (patternCoroutine == null)
        {
            Debug.Log("보스전 시작 준비: 4초 후 패턴 시작");
            StartCoroutine(StartBossFightWithDelay(0.1f));
            bossHPSlider.gameObject.SetActive(true);
        }
    }

    protected virtual IEnumerator StartBossFightWithDelay(float delay)
    {
        
 
        yield return new WaitForSeconds(delay);

 
        isTeleportable = true;
        patternCoroutine = StartCoroutine(BossPatternMaster());
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

        isAttacking = false;
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


    public void OnDamaged(float damage)
    {
        bossCurrentHP -= damage;
        if (bossCurrentHP <= 0)
        {
            BossClear();
        }
    }
    protected virtual void BossClear()
    {
        GameObject[] enemyAttack = GameObject.FindGameObjectsWithTag("BossAttack");
        
        foreach (GameObject Attack in enemyAttack)
        {
 
            Destroy(Attack);
        }
        StopAllCoroutines();
        soundPlayer.SoundPlay(9);

        uiManager.FadeOutStart();
        yellowBoss.BossClearCamera();
        Invoke("SceneLoad",2.5f);
    }
    void SceneLoad()
    {
        GameManager.Instance.playerInstanceCurrentHP = GameObject.Find("Player").GetComponent<PlayerCombat>().currentHP;
        GameManager.Instance.playerInstanceCurrentST = GameObject.Find("Player").GetComponent<PlayerCombat>().currentST;
        SceneManager.LoadScene("Stage04.5");
    }

    protected virtual IEnumerator GroggyCoroutine()
    {
        isGroggy = true;
        anim.SetTrigger("Groggy");
        anim.SetBool("isGroggy", true);
        groggyCoroutineStart();
        boxCollider.enabled = true;
        yield return new WaitForSeconds(5f);
        crystalLife = 3;
        Instantiate(bossCrystal[0], new Vector3(8, 1.3f, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(bossCrystal[1], new Vector3(14, -0.77f, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(bossCrystal[2], new Vector3(20, 1.3f, 0), Quaternion.Euler(0, 0, 0));
        isGroggy = false;
        ForcedSideTeleport();
        boxCollider.enabled = false;
        anim.SetBool("isGroggy", false);
        anim.ResetTrigger("Groggy");
    }
    protected virtual void groggyCoroutineStart()
    {
        if (bossLocation != 0)
        {
            if (groggyMove != null)
            {
                StopCoroutine(groggyMove);
            }
            groggyMove = StartCoroutine(GroggyMovement(1));
        }
        else if (bossLocation == 0)
        {
            if (groggyMove != null)
            {
                StopCoroutine(groggyMove);
            }
            groggyMove = StartCoroutine(GroggyMovement(0));
        }
    }
    private Coroutine groggyMove;
    protected virtual IEnumerator GroggyMovement(int location)
    {
        Vector3 startPosition = transform.position;
        Vector3 groggyLocation;
        if (location == 0)
        {
            groggyLocation = gameObject.transform.position + new Vector3(0, -3.5f, 0);
        }
        else
        {
            groggyLocation = gameObject.transform.position + new Vector3(0, -5.5f, 0);
        }
        float duration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, groggyLocation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = groggyLocation;

    }
    protected virtual void UpdatePosition()
    {
 
        if (target != null)
        {
 
            playerPosition = target.position;
  
        }
    }
    protected virtual IEnumerator BossPatternMaster()
    {
        yield return new WaitForSeconds(1f); 

        while (true) 
        {
 
            if (isTeleportable && !isGroggy)
            {
                Debug.Log("텔포했어요");
                anim.ResetTrigger("Spine");
                anim.Play("Y_Boss_Idle");
                Teleport();
                GameObject bossTPEffect = Instantiate(bossTeleportEffect, gameObject.transform);
                Destroy(bossTPEffect, 1f);
                isTeleportable = false;
                
            }

            yield return new WaitForSeconds(0.5f);

            if (bossLocation == 0)
            {
 
                Spine();
                
 
                while (isAttacking)
                {
                    yield return null;
                }

 
                ForcedSideTeleport();
                anim.ResetTrigger("Spine");
                GameObject bossTPEffect = Instantiate(bossTeleportEffect, gameObject.transform);
                Destroy(bossTPEffect, 1f);
            }
            else 
            {
                float attackTimer = 0f;
                while (attackTimer < teleportCoolTime - 0.5f)
                {
                    if (!isGroggy && !isAttacking)
                    {
                        PatternCalculation();
                    }

                    yield return new WaitForSeconds(attackCoolTime);
                    attackTimer += attackCoolTime;

                    if (attackTimer >= teleportCoolTime) break;
                }
            }
            
 
            isTeleportable = true;
        }
    }
    protected virtual void Teleport()
    {
 

        if (playerPosition.x <= 12)
        {
            gameObject.transform.position = new Vector3(4f, 2, 0);
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            bossLocation = -1; 
        }
        else if (playerPosition.x >= 16)
        {
            gameObject.transform.position = new Vector3(24f, 2, 0);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            bossLocation = 1; 
        }
        else
        {
            gameObject.transform.position = new Vector3(14f, 0f, 0);
            bossLocation = 0; 
 
            if (playerPosition.x >= gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (playerPosition.x < gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        soundPlayer.SoundPlay(8);
    }
    protected virtual void AttackingCheck(int value)
    {
        isAttacking = value == 0 ? true : false;
    }
   protected virtual  IEnumerator AttackCoolDown()
    {
        isAttackCool = true;
        yield return new WaitForSeconds(attackCoolTime);
        isAttackCool = false;
    }

    protected virtual void ForcedSideTeleport()
    {
        Debug.Log("텔레포트 강제");
 
        int randSide = (UnityEngine.Random.Range(0, 2) * 2) - 1;

        if (randSide == -1) 
        {
            gameObject.transform.position = new Vector3(4f, 2, 0);
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            GameObject bossTPEffect = Instantiate(bossTeleportEffect, gameObject.transform);
            Destroy(bossTPEffect, 1f);
            bossLocation = -1;
        }
        else 
        {
            gameObject.transform.position = new Vector3(24f, 2, 0);
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            GameObject bossTPEffect = Instantiate(bossTeleportEffect, gameObject.transform);
            Destroy(bossTPEffect, 1f);
            bossLocation = 1;
        }
        soundPlayer.SoundPlay(8);
    }

    protected virtual void PatternCalculation()
    {
        if (bossLocation != 0) 
        {
            int rand = UnityEngine.Random.Range(0, 2); 

            if (rand == 0) 
            {
 
                StartCoroutine(ExecuteAttack(() => Meteor(bossLocation, playerPosition), 0.5f));
                anim.SetTrigger("Meteor");
                soundPlayer.SoundPlay(3);
            }
            else 
            {
 
                StartCoroutine(ExecuteAttack(() => Sphere(bossLocation), 0.5f));
                anim.SetTrigger("Sphere");
                soundPlayer.SoundPlay(4);
            }
        }
        else if (bossLocation == 0) 
        {
 
            if (!isAttacking)
            {
                Spine();
                anim.SetTrigger("Spine");
            }
        }
    }
    protected virtual IEnumerator ExecuteAttack(System.Action attackAction, float duration)
    {
 
        isAttacking = true; 
 
        anim.SetBool("isAttacking", true);
        attackAction.Invoke();

 
        yield return new WaitForSeconds(duration);

        isAttacking = false; 
        anim.SetBool("isAttacking", false);
    }
          protected virtual void ThunderPattern()
    {
        int patternValue = Random.Range(0, 2);
        Debug.Log(patternValue);
        StartCoroutine(ThunderInstantiate(patternValue));
    }

    protected virtual void Meteor(int direction, Vector3 playerPosition)
    {


        if (direction == -1) 
        {
 
            meteorSpawnPosition = new Vector3(4f, 10f, 1f);
        }
        else if (direction == 1) 
        {
 
            meteorSpawnPosition = new Vector3(23f, 10f, 1f);
        }
        Vector3 directionVector = playerPosition - meteorSpawnPosition;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        GameObject newMeteorObject = Instantiate(bossMeteor, meteorSpawnPosition, targetRotation);

        Yellow_Boss_Meteor meteorScript = newMeteorObject.GetComponent<Yellow_Boss_Meteor>();
        if (meteorScript != null)
        {
            meteorScript.SetTarget(playerPosition); 
        }
    }
    protected virtual void Sphere(int direction)
    {
        if (direction == -1)
        {
            spherepawnPosition = gameObject.transform.position + new Vector3(4, 2, 0);
        }
        else if (direction == 1)
        {
            spherepawnPosition = gameObject.transform.position + new Vector3(-4, 2, 0);
        }
        GameObject newSphereObject = Instantiate(bossSphere, spherepawnPosition, Quaternion.identity);
        Yellow_Boss_Sphere sphereScript = newSphereObject.GetComponent<Yellow_Boss_Sphere>();
        if (sphereScript != null)
        {
            sphereScript.SetTarget(target);
        }
    }
    protected virtual void Spine()
    {
        if (isAttacking) return; 

        activeSpines.Clear();
        StartCoroutine(SpineCoroutine());
    }
    protected virtual IEnumerator SpineCoroutine()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetTrigger("Spine");
        activeSpines.Clear();
        

        for (int i = 0; i < spineAngles.Length; i++)
        {
            float targetAngle = spineAngles[i];

            Vector3 spawnPosition = spineSpawnPoint[i];

            GameObject spineObject = Instantiate(bossSpine, spawnPosition, Quaternion.Euler(0f, 0f, targetAngle));

            soundPlayer.SoundPlay(5);

            Yellow_Boss_Spine spineScript = spineObject.GetComponent<Yellow_Boss_Spine>();
            if (spineScript != null)
            {
                spineScript.SetTarget(target);
                activeSpines.Add(spineScript);
            }

            yield return new WaitForSeconds(0.5f);
        }
        foreach (var spine in activeSpines)
        {
            if (spine != null)
            {
                spine.StartAlignment();
            }
        }
        float spineDelay = Random.Range(1, 3f);
        yield return new WaitForSeconds(spineDelay);

        for (int i = 0; i < activeSpines.Count; i++)
        {
            Yellow_Boss_Spine spine = activeSpines[i];

            if (spine != null)
            {
                spine.Launch();
                soundPlayer.SoundPlay(6);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.0f); 

 
        isAttacking = false; 
        anim.SetBool("isAttacking", false);
 
        Debug.Log("Spine Pattern Completed. isAttacking is now FALSE.");
        
 
        activeSpines.Clear(); 
    }


    protected virtual IEnumerator ThunderInstantiate(int pattern)
    {
        if (pattern == 0)
        {
            GameObject precursor00 = Instantiate(PatternPrecursor[0]);
            yield return new WaitForSeconds(1f);
            Destroy(precursor00);
            GameObject ThunderPattern00 = Instantiate(bossThunder[0]);
            soundPlayer.SoundPlay(7);
            yield return new WaitForSeconds(1f);
            Destroy(ThunderPattern00);
        }
        else if (pattern == 1)
        {
            GameObject precursor01 = Instantiate(PatternPrecursor[1]);
            yield return new WaitForSeconds(1f);
            Destroy(precursor01);
            GameObject ThunderPattern01 = Instantiate(bossThunder[1]);
            soundPlayer.SoundPlay(7);
            yield return new WaitForSeconds(1f);
            Destroy(ThunderPattern01);
        }
    }
    public virtual void ThunderStart()
    {
        StartCoroutine(RandomlyThunder());
    }
    protected virtual IEnumerator RandomlyThunder()
    {
        while (true)
        {
 
            float randomDelay = Random.Range(thunderMinDelay, thunderMaxDelay);

            Debug.Log($"다음 실행까지 {randomDelay:F2}초 대기합니다.");

 
            yield return new WaitForSeconds(randomDelay);

 
            ThunderPattern();
        }
    }
    

}
