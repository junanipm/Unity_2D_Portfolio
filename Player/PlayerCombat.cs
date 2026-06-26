using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayerCombat : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider;
    public Animator anim;
 
    public float playerMaxHP = 100;
    public float currentHP;
    public float playerMaxST = 50;
    public float currentST;
    public float stPerSec = 5;
    public float attackPower = 10;



    private bool isAttackBuffered;
    private bool isArrowLoading;
    private bool isArrowRainReady = true;
    private bool isSamshingReady = true;

    public bool isAttacking;
    private bool comboEnable;
    private bool comboExist;
    public bool isDied { get; private set; }
    public bool isDamaged { get; private set; }
    private int attackCount;
    public RangeCheck rangeCheck;
    public P_SkillRangeCheck skillRangeCheck;

    public float slashCoolTime = 3;

    float targetX;
    bool isSlashable;
    [Header("파랑이용")]
    public GameObject strikeEffect;
    public GameObject slashEffect;
    public bool isSkillUsing;

    [Header("원거리용")]
    public GameObject arrowPrefab;
    public Transform firePoint;
    public GameObject y_SkillPrefab;
    public GameObject y_SkillAnimObject;

    [Header("보라용")]
    public bool isCharging { get; private set; } = false;
    public GameObject purpleEffect;
    public GameObject smashHitEffect;
    float fullChargeThreshold = 2f;
    private int chargingProcess = 0;
    private float purpleChargeStartTime;
    private Coroutine purpleChargeCoroutine;

 


    KeyCode attackKey = KeyCode.LeftControl;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    PlayerController playerController;

    Animator animator;
    AnimatorConverter animConverter;
    public CameraController cameraController;
    private SkillCoolManager skillCoolManager;

    public GameObject healEffect;

    [Header("기타")]
    public bool isKnockedBack { get; private set; } = false;

    private float knockbackTime = 0.2f;
    [SerializeField]
    private float graceTime = 0.7f;
    [SerializeField]
    float originalGravity = 8;
    public AudioClip blueSkillHit;
    public AudioClip y_s;
    float yellowSkillVec;
    
    
    void Awake()
    {
        skillCoolManager = GetComponent<SkillCoolManager>();

    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        rangeCheck = GetComponentInChildren<RangeCheck>();
        skillRangeCheck = GetComponentInChildren<P_SkillRangeCheck>();
        animator = GetComponent<Animator>();
        animConverter = GetComponent<AnimatorConverter>();
        cameraController = GameObject.Find("Virtual Camera").GetComponent<CameraController>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        currentHP = GameManager.Instance.playerInstanceCurrentHP;
        currentST = GameManager.Instance.playerInstanceCurrentST;

        attackCount = 0;

        isAttacking = false;
        isAttackBuffered = false;
        animator.SetInteger("CurrentMode", (int)animConverter.currentState);

        isSlashable = true;
        isDied = false;
    }

    void Update()
    {
        if (isDied || playerController.playerPaused || GameManager.Instance.isPaused) return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (animConverter.currentState == PlayerState.Blue)
            {
                if (!isAttackBuffered)
                {
                    anim.SetTrigger("Attack");
                }
            }
            else if (animConverter.currentState == PlayerState.Yellow)
            {
 
 
                if (!isArrowLoading)
                {
                    anim.SetTrigger("Attack");
 
                }
                    

            }
            else if (animConverter.currentState == PlayerState.Purple)
            {
                if (!isAttacking)
                {
                    anim.SetTrigger("Attack");
 
                }
            }
                else if (animConverter.currentState == PlayerState.White)
                {
                if (!isAttackBuffered)
                {
                    anim.SetTrigger("Attack");
 
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(animConverter.currentState == PlayerState.Yellow)
                anim.ResetTrigger("Attack");
        }
        

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (animConverter.currentState == PlayerState.Blue)
                BlueSkill();
            else if (animConverter.currentState == PlayerState.Yellow)
            {
                if (isArrowRainReady && !isDamaged && !playerController.isDashing && !isAttacking)
                {
 
                    yellowSkillVec = playerController.lastMoveDirc;
                    anim.SetTrigger("Skill");
                    y_SkillAnimObject.SetActive(true);
                }
            }
            else if (animConverter.currentState == PlayerState.Purple)
            {
                PurpleSkill();
                anim.SetTrigger("Skill");
            }
        }
        if (animConverter.currentState == PlayerState.Purple)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1) && isCharging)
            {
                OnPurpleSkillReleased();
 
            }



        }






        StaminaRecovry();
        Vector2 direction = new Vector2(playerController.lastMoveDirc, 0).normalized;


    }

    void SetLayer()
    {
        if (playerController.isDashing || isDamaged || isDied)
        {
            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 7;
        }
    }

    void SetAlpha()
    {
        if (isDamaged)
        {
            StartCoroutine(AlphaBlinking());
        }
        else
        {
            spriteRenderer.material.SetFloat("_Alpha", 1f);
        }
    }
    IEnumerator AlphaBlinking()
    {
        float damagedTime = knockbackTime + graceTime;
        float blinkDuration = damagedTime;
        float blinkSpeed = 2.5f;

        float elapsed = 0;

        while (elapsed < damagedTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed * blinkSpeed, 1f);
            float alpha = Mathf.Lerp(0.2f, 0.8f, t);

            spriteRenderer.material.SetFloat("_Alpha", alpha);

            yield return null;
        }
    }

    void AttackEnemies()
    {
        foreach (Enemy enemy in rangeCheck.enemiesInRange)
        {
            if (animConverter.currentState == PlayerState.Blue)
            {
                float rotationValue = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

                enemy.EnemyOnDamaged(attackPower, 3);
                GameObject effect = Instantiate(strikeEffect, enemy.gameObject.transform.position, randomRotation);
                Destroy(effect, 0.32f);
            }
            else if (animConverter.currentState == PlayerState.Purple)
            {
                float rotationValue = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

                enemy.EnemyOnDamaged(attackPower * 3.5f, 3);
                GameObject effect = Instantiate(smashHitEffect, enemy.gameObject.transform.position, randomRotation);
                Destroy(effect, 0.32f);
            }

        }
        foreach (BlueBossCombat boss in rangeCheck.bossesInRange)
        {
            float rotationValue = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

 
            boss.OnDamaged(attackPower);
            GameObject effect = Instantiate(strikeEffect, boss.transform.position, randomRotation);
            Destroy(effect, 0.32f);
        }

        foreach (YellowBossCombat yellowBoss in rangeCheck.yBossInRanage)
        {
            float rotationValue = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

 
            yellowBoss.OnDamaged(attackPower);
            GameObject effect = Instantiate(strikeEffect, yellowBoss.transform.position, randomRotation);
            Destroy(effect, 0.32f);
        }

        foreach (FinalBossCombat finalBossCombat in rangeCheck.fBossInRanage)
        {
            if (finalBossCombat == null) return;
            if (animConverter.currentState == PlayerState.Blue)
                {
                    float rotationValue = Random.Range(0f, 360f);
                    Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

                    finalBossCombat.OnDamaged(attackPower);
                    GameObject effect = Instantiate(strikeEffect, finalBossCombat.gameObject.transform.position, randomRotation);
                    Destroy(effect, 0.32f);
                }
                else if (animConverter.currentState == PlayerState.Purple)
                {
                    float rotationValue = Random.Range(0f, 360f);
                    Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

                    finalBossCombat.OnDamaged(attackPower * 3.5f);
                    GameObject effect = Instantiate(smashHitEffect, finalBossCombat.gameObject.transform.position, randomRotation);
                    Destroy(effect, 0.32f);
                }

        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Trap" && !playerController.isDashing && !isDied)
        {
            TakeDamage(collision.transform.position, 10);
        }
        else if (collision.gameObject.tag == "BossAttack" && !playerController.isDashing && !isDied)
        {
            BossEffect bossEffect = collision.gameObject.GetComponent<BossEffect>();
            TakeDamage(collision.transform.position, bossEffect.damage);

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyAttack" && !playerController.isDashing && !isDied)
        {
            
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            if(enemy != null) TakeDamage(other.transform.position, enemy.enemyAttackP);


            
        }
        
        
    }




    public void TakeDamage(Vector2 targetPos, float damage)
    {
        bool isSuperArmor = (animConverter.currentState == PlayerState.Purple);

        isDamaged = true;
        currentHP -= damage;


        if (currentHP > 0)
        {
            if (!isSuperArmor)
            {
                anim.SetTrigger("isDamaged");
            }

        }
        else if (currentHP <= 0 && !isDied)
        {
            anim.SetTrigger("Die");
            isDied = true;
            gameObject.layer = 9;

            if (isCharging)
            {
                if (purpleChargeCoroutine != null)
                {
                    StopCoroutine(purpleChargeCoroutine);
                    purpleChargeCoroutine = null;
                }
                PurpleSkillEnd();
            }

            playerController.PlayerDie();
        }
        if (!isSuperArmor && !isDied)
        {
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
 
            rigid.linearVelocity = Vector2.zero;

            isKnockedBack = true;
            StartCoroutine(KnockBack(dirc));
                          cameraController.StopShake();
        }
        else
        {
            StartCoroutine(PurpleStateDamageEffect());
        }   
        

        
    }

    IEnumerator KnockBack(int dirc)
    {
        SetLayer();
        SetAlpha();
 
        rigid.linearVelocity = Vector2.zero;

        rigid.linearVelocity = new Vector2(dirc * 1.5f, 2);

        yield return new WaitForSeconds(knockbackTime);

        isKnockedBack = false;
        isDamaged = false;
        isAttacking = false;
        isSkillUsing = false;

        yield return new WaitForSeconds(graceTime);
        isDamaged = false;
        if (isAttacking) isAttacking = false;
 
        SetAlpha();
        SetLayer();
    }
    IEnumerator PurpleStateDamageEffect()
    {
        SetLayer();
        SetAlpha();

        yield return new WaitForSeconds(knockbackTime);
        isDamaged = false;
        yield return new WaitForSeconds(graceTime);
        isDamaged = false;
        SetAlpha();
        SetLayer();
    }

    void StaminaRecovry()
    {

        if (currentST != playerMaxST)
        {
            currentST += Time.deltaTime * stPerSec;
        }

        if (currentST > playerMaxST)
        {
            currentST = playerMaxST;
        }
    }
    void AttackDash()
    {
 
        rigid.linearVelocityX = playerController.lastMoveDirc * 4.5f;
    }
    void ComboEnable()
    {
        
        comboEnable = true;

    }
    void ComboDisable()
    {
        comboEnable = false;
    }
    void ComboExist()
    {
        if (comboExist == false) return;
        comboExist = false;

        attackCount++;
        animator.SetTrigger("Attack");
    }

    void ComboExistWait()
    {
        StartCoroutine(ComboWaitTime());
    }
    IEnumerator ComboWaitTime()
    {
        float waitTime;
        if (animConverter.currentState == PlayerState.White)
        {
            waitTime = 0.75f;
        }
        else
        {
            waitTime = 0.6f;
        }
        var WaitForSeconds = new WaitForSeconds(waitTime);
        isAttackBuffered = true;

        yield return WaitForSeconds;
        isAttackBuffered = false;

    }
    void RangeAttack()
    {

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, transform.rotation);
        arrow.GetComponent<Projectile>().Init(attackPower, playerController.lastMoveDirc * firePoint.right);
        StartCoroutine(RangeAttackDelay());
    }
    IEnumerator RangeAttackDelay()
    {
        var WaitForSeconds = new WaitForSeconds(0.6f);
        isArrowLoading = true;
        yield return WaitForSeconds;
        isArrowLoading = false;

    }
    void BlueSkill()
    {
        if (isSlashable && !isDamaged && !playerController.isDashing && !isAttacking)
        {
            PlayerState currentState = skillCoolManager.GetCurrentState();
            if (skillCoolManager.CanUseSkill(currentState))
            {
                StartCoroutine(Slashing());
                skillCoolManager.StartCooldown(currentState);
                StartCoroutine(SlashCoolDown(skillCoolManager.GetCooldownDuration(currentState)));
            }

        }

    }
    public void YellowSkill()
    {
        if (isArrowRainReady && !isDamaged && !playerController.isDashing && !isAttacking)
        {
            float vec = yellowSkillVec;
            
            PlayerState currentState = skillCoolManager.GetCurrentState();
            if (skillCoolManager.CanUseSkill(currentState))
            {
                currentST -= 20;
                StartCoroutine(ArrowRain(vec));
                skillCoolManager.StartCooldown(currentState);
                StartCoroutine(ArrowRainCoolDown(skillCoolManager.GetCooldownDuration(currentState)));

            }
        }
    }
    void YellowSkillDisabled()
    {
        y_SkillAnimObject.SetActive(false);
    }
    IEnumerator Slashing()
    {
        isSkillUsing = true;
        float slashDistance = 3;
        float slashSpeed = 150;
        float movedDistance = 0f;
        float originalDrag = rigid.linearDamping;

        Vector2 direction = new Vector2(playerController.lastMoveDirc, 0).normalized;
        Vector2 startPosition = transform.position;

        playerController.isDashing = true;
        gameObject.layer = 9;
 
        rigid.linearVelocity = Vector2.zero;
        currentST -= 20;
        rigid.gravityScale = 0;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        anim.SetTrigger("Skill");
        rigid.linearDamping = 0;


        yield return new WaitForSeconds(0.2f);

        rigid.linearVelocity = direction * slashSpeed;
        float checkDistance = 0.7f;
        while (movedDistance < slashDistance)
        {
              
            Vector2 origin = (Vector2)transform.position + capsuleCollider.offset;
            Vector2 size = new Vector2(capsuleCollider.size.x, 0.8f);


            movedDistance = Vector2.Distance(startPosition, transform.position);
            RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f,
            direction, checkDistance, LayerMask.GetMask("Wall", "Platform", "Board"));







            if (hit.collider != null)
            {
                Debug.Log("Wall or Platform Detacted");
                break;
            }
            yield return null;
        }

        rigid.linearVelocity = Vector2.zero;
        Vector2 endPosition = transform.position;
        yield return new WaitForSeconds(0.3f);
        rigid.gravityScale = originalGravity;


        rigid.linearDamping = originalDrag;



        Vector2 center = (startPosition + endPosition) / 2f;
        float distance = Vector2.Distance(startPosition, endPosition);
        float height = 3f;
 

 
 
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(center, new Vector2(distance, height), 0, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy dmg = enemy.GetComponent<Enemy>();
            if (dmg != null)
            {
                dmg.EnemyOnDamaged(attackPower * 2, 3);

                float rotationValue = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

                SoundManager.instance.SFXPlay("hit", blueSkillHit);
                GameObject effect = Instantiate(strikeEffect, enemy.gameObject.transform.position, randomRotation);
                GameObject slEffect = Instantiate(slashEffect, enemy.gameObject.transform.position, randomRotation);
                Destroy(effect, 0.32f);
                Destroy(slEffect, 0.5f);
            }

        }
        foreach (Collider2D boss in hitEnemies)
        {
            YellowBossCombat dmg_boss = boss.GetComponent<YellowBossCombat>();
            if (dmg_boss != null)
            {
                dmg_boss.OnDamaged(attackPower * 2);

                float rotationValue = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);

                SoundManager.instance.SFXPlay("hit", blueSkillHit);
                GameObject effect = Instantiate(strikeEffect, boss.gameObject.transform.position, randomRotation);
                GameObject slEffect = Instantiate(slashEffect, boss.gameObject.transform.position, randomRotation);
                Destroy(effect, 0.32f);
                Destroy(slEffect, 0.5f);
            }
        }

        foreach (Collider2D fBoss in hitEnemies)
        {
            FinalBossCombat dmg_boss = fBoss.GetComponent<FinalBossCombat>();
            if (dmg_boss != null)
            {
                dmg_boss.OnDamaged(attackPower * 2);

                float rotationValue = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);
                SoundManager.instance.SFXPlay("hit", blueSkillHit);
                GameObject effect = Instantiate(strikeEffect, fBoss.gameObject.transform.position, randomRotation);
                GameObject slEffect = Instantiate(slashEffect, fBoss.gameObject.transform.position, randomRotation);
                Destroy(effect, 0.32f);
                Destroy(slEffect, 0.5f);
            }
        }

        playerController.isDashing = false;
        yield return new WaitForSeconds(0.5f);
        SetLayer();
        isSkillUsing = false;
    }
    IEnumerator SlashCoolDown(float cool)
    {

        isSlashable = false;
        yield return new WaitForSeconds(cool);
        isSlashable = true;

    }

    IEnumerator ArrowRain(float vec)
    {
        

        isArrowRainReady = false;
        float launchDistance = 7f;
        Vector2 launchPoint = new Vector2(gameObject.transform.position.x + vec * launchDistance, gameObject.transform.position.y + 2f);
        SoundManager.instance.SFXPlay("Y_s", y_s);
        GameObject arrowRain = Instantiate(y_SkillPrefab, launchPoint, transform.rotation);
        arrowRain.GetComponent<YellowSkill>().Init(attackPower, 0.15f);
        yield return null;
    }
    IEnumerator ArrowRainCoolDown(float cool)
    {
        isArrowRainReady = false;

        yield return new WaitForSeconds(cool);
        isArrowRainReady = true;
    }

    void PurpleSkill()
    {
        if (isSamshingReady && !isDamaged && !playerController.isDashing && !isAttacking)
        {
            PlayerState currentState = skillCoolManager.GetCurrentState();
            if (skillCoolManager.CanUseSkill(currentState))
            {
                currentST -= 20;
                isCharging = true;
                purpleChargeStartTime = Time.time;
                purpleChargeCoroutine = StartCoroutine(PurpleSkillCorutine()); 
                
                ShakeCam(0.25f);

                isSkillUsing = true;

                skillCoolManager.StartCooldown(currentState);
                StartCoroutine(PurpleSkillCooldown(skillCoolManager.GetCooldownDuration(currentState)));
            }
        }
    }


    void OnPurpleSkillReleased()
    {
        isCharging = false;
        anim.SetBool("isCharging", false);
        StopCoroutine(purpleChargeCoroutine); 
        StopShake();
        purpleEffect.SetActive(false);

 
        float chargedTime = Time.time - purpleChargeStartTime;
        int damageValue = 0;
        if (chargedTime >= fullChargeThreshold)
        {
            damageValue = 2;
        }
        else if (chargedTime >= fullChargeThreshold / 2)
        {
            damageValue = 1;
        }
        else
        {
            damageValue = 0; 
        }
        anim.SetInteger("chargingProcess", chargingProcess);

 
        skillRangeCheck.processCheck(damageValue);
        SkillDamagePerTime(damageValue);

        isSkillUsing = false;

 
        skillRangeCheck.enemiesInSkillRange.Clear();
        skillRangeCheck.finalBossInSkillRange.Clear();

 
        skillRangeCheck.processCheck(0);
    }


    IEnumerator PurpleSkillCooldown(float cool)
    {

        isSamshingReady = false;
        yield return new WaitForSeconds(cool);
        isSamshingReady = true;
    }

    IEnumerator PurpleSkillCorutine()
    {

        skillRangeCheck.processCheck(1); 
        anim.SetBool("isCharging", true);

        while (isCharging)
        {
            float elapsedTime = Time.time - purpleChargeStartTime;
            anim.SetFloat("chargingTime", elapsedTime);

            if (elapsedTime >= fullChargeThreshold)
            {
                if (chargingProcess != 2)
                {
                    ShakeCam(1);
                    chargingProcess = 2;
                }
            }
            else if (elapsedTime >= fullChargeThreshold / 2)
            {
                if (chargingProcess != 1)
                {
                    chargingProcess = 1;
                }
            }
            else
            {
                if (chargingProcess != 0)
                {
                    chargingProcess = 0;
                }
            }
            yield return null;
        }
    }
    void SkillDamagePerTime(int value)
    {
        Debug.Log("보라스킬");
        Debug.Log($"현재 스킬 범위 내 적의 수: {skillRangeCheck.enemiesInSkillRange.Count}");
        foreach (Enemy enemy in skillRangeCheck.enemiesInSkillRange)
        {
            Debug.Log("보라스킬22");
            float rotationValue = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);
            if (value == 1) enemy.EnemyOnDamaged(attackPower * 4, 20);
            else if (value == 2) enemy.EnemyOnDamaged(attackPower * 8, 20);
            GameObject effect = Instantiate(smashHitEffect, enemy.gameObject.transform.position, randomRotation);
            Destroy(effect, 0.32f);
        }

        foreach (FinalBossCombat fBoss in skillRangeCheck.finalBossInSkillRange)
        {
            Debug.Log("보라스킬22");
            float rotationValue = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, rotationValue, 0);
            if (value == 1) fBoss.OnDamaged(attackPower * 4);
            else if (value == 2) fBoss.OnDamaged(attackPower * 8);
            GameObject effect = Instantiate(smashHitEffect, fBoss.gameObject.transform.position, randomRotation);
            Destroy(effect, 0.32f);
        }
    }



    void PurpleSkillEnd()
    {
        isCharging = false;
        anim.SetFloat("chargingTime", 0);
        anim.SetBool("isCharging", false);
        purpleEffect.SetActive(false);
        chargingProcess = 0;
        skillRangeCheck.processCheck(0);
        StopShake();
    }
    void PurpleFullSkil()
    {
        anim.SetFloat("chargingTime", 0);
    }
    void PurpleEffectToggle(int i)
    {
        bool toggle;
        toggle = i == 0 ? true : false;
        purpleEffect.SetActive(toggle);
    }

    public void AttackBool(int value)
    {

        isAttacking = value == 0 ? true : false;

    }

    public void SkillBool(int value)
    {

        isSkillUsing = value == 0 ? true : false;

    }
    void ShakeCam(float a)
    {
        cameraController.ShakeCamera(a, 1f);
    }
    void StopShake()
    {
        cameraController.StopShake();
    }
    public void healEffectActive()
    {
        GameObject healEff = Instantiate(healEffect, gameObject.transform);
        Destroy(healEff, 1f);
    }
}
