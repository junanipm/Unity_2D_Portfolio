<<<<<<< HEAD
using System.Collections;
using System.ComponentModel.Design;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FinalBossCombat : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected AnimatorConverter animatorConverter;
    protected UIManager uiManager;
    protected BoxCollider2D boxCollider;
    FinalBossAudio finalBossAudio;
    [SerializeField]
    protected Transform target;
    protected Vector3 playerPosition;
    private int playerLocationIntigger;
    bool isOpenning = true;

    public bool isAttacking;

    public bool isBasicAttackCool;
    public bool isSkillCool;
    [SerializeField]
    float attackCooldown = 1f;
    [SerializeField]
    float skillCooldown = 5f;

    [SerializeField]
    protected float bossMaxHP = 500;
    [SerializeField]
    protected float bossCurrentHP = 500;

    public int BossPhase;
    bool bossCleared = false;
    [SerializeField]
    GameObject dashEffect;
    [SerializeField]
    GameObject veriticalSlash;
    [SerializeField]
    GameObject horizontalSlash;
    [SerializeField]
    GameObject[] attackCol;
    [SerializeField]
    GameObject hammerEffect;
    [SerializeField]
    GameObject arrowEffect;
    [SerializeField]
    GameObject arrowRainEffect;

    public Slider bossHPSlider;
    [SerializeField]
    protected float moveSpeed = 2.5f;
    [SerializeField]
    protected float minDistanceToPlayer = 3f;
    [SerializeField]
    protected float maxDistanceToPlayer = 10f;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isAttacking = false;
        isBasicAttackCool = false;
        isSkillCool = false;

        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        animatorConverter = GameObject.Find("Player").GetComponent<AnimatorConverter>();

        InvokeRepeating("UpdatePosition", 0.05f, 0.5f);
        BossPhase = 0;


        InvokeRepeating("TrySkillPattern", 0.5f, 2.5f);
        BossPhase = 0;
        finalBossAudio = GetComponent<FinalBossAudio>();
    }


    void Update()
    {
        bossHPSlider.maxValue = bossMaxHP;
        bossHPSlider.value = bossCurrentHP;
        if (!bossCleared)
        {
            if (isAttacking || isOpenning)
            {
                anim.SetBool("isWalking", false);
                return; 
            }

            if (target == null)
            {
                anim.SetBool("isWalking", false);
                return;
            }


            HandleMovementAndFlip();
            if (bossCurrentHP <= bossMaxHP / 2)
            {
                BossPhase = 1;
                Debug.Log("bossPhase:" + BossPhase);
            }
        }

    }


    public void FinalBossCombatStart()
    {
        isOpenning = false;
        boxCollider.enabled = true;
        StartCoroutine(LandingCoroutine());
        Invoke("SliderActive", 2.5f);
    }
    public void SliderActive()
    {
        bossHPSlider.gameObject.SetActive(true);
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
        bossCleared = true;
        anim.SetTrigger("Die");
        boxCollider.enabled = false;
        bossHPSlider.gameObject.SetActive(false);

        StopAllCoroutines();

        CancelInvoke("UpdatePosition");
        CancelInvoke("TrySkillPattern");
        GameObject[] enemyAttack = GameObject.FindGameObjectsWithTag("BossAttack");

        foreach (GameObject Attack in enemyAttack)
        {

            Destroy(Attack);
        }
        StartCoroutine(FadeOutStart());
        Invoke("ClearSceneChange", uiManager.fadeDuration+4.5f);
    }
    IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(2f);
        uiManager.FadeOutStart();
    }
    void ClearSceneChange()
    {
        GameManager.Instance.playerInstanceCurrentHP = GameObject.Find("Player").GetComponent<PlayerCombat>().currentHP;
        GameManager.Instance.playerInstanceCurrentST = GameObject.Find("Player").GetComponent<PlayerCombat>().currentST;
        SceneManager.LoadScene("Stage10");
    }

    protected virtual void UpdatePosition()
    {

        if (target != null)
        {

            playerPosition = target.position;
            if (playerPosition.x >= 0)
            {
                playerLocationIntigger = 1;
            }
            else
            {
                playerLocationIntigger = -1;
            }

            if (isAttacking)
            {
                return;
            }


        }
    }
    protected void HandleMovementAndFlip()
    {

        if (!isOpenning)
        {

            Vector3 currentScale = transform.localScale;


            if (playerPosition.x > transform.position.x)
            {
                currentScale.x = -1; 
            }

            else if (playerPosition.x < transform.position.x)
            {
                currentScale.x = 1; 
            }
            transform.localScale = currentScale;

            float distanceX = Mathf.Abs(playerPosition.x - transform.position.x);
            if (distanceX > maxDistanceToPlayer && !isSkillCool)
            {
                TeleportToPlayer();
            }

            else if (distanceX > minDistanceToPlayer)
            {
                
                anim.SetBool("isWalking", true);

                Vector3 targetPosition = new Vector3(playerPosition.x, transform.position.y, transform.position.z);

                transform.position = Vector3.MoveTowards(
                    transform.position,    
                    targetPosition,        
                    moveSpeed * Time.deltaTime 
                );
            }

            else
            {

                anim.SetBool("isWalking", false);


                if (!isSkillCool)
                {

                    TrySkillPattern();
                }

                else if (!isBasicAttackCool)
                {
                    Attack();
                }
            }
        }
    }

    protected void TrySkillPattern()
    {
        
        if (isAttacking || isSkillCool || isOpenning)
        {
            return;
        }


        if (BossPhase == 0)
        {
            FirstPhasePatterCalculation();
        }
        else if (BossPhase == 1)
        {
            SecondPhasePaterenCalculation();
        }
    }


    protected void FirstPhasePatterCalculation()
    {

        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < 23)
        {

            StartCoroutine(LandingCoroutine());
        }

        else if (rand < 46)
        {
            StartCoroutine(HorizontalSlash()); 
        }

        else if (rand < 64)
        {

            StartCoroutine(VerticalSlashCoroutine());
        }

        else if (rand < 82)
        {

            StartCoroutine(ArrowCoroutine());
        }

        else
        {

            StartCoroutine(DashCoroutine());
        }
    }
    void SecondPhasePaterenCalculation()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 15)
        {
            StartCoroutine(LandingCoroutine());
        }
        else if (rand < 30)
        {
            StartCoroutine(HorizontalSlash());
        }
        else if (rand < 44)
        {
            StartCoroutine(VerticalSlashCoroutine());
        }
        else if (rand < 58)
        {
            StartCoroutine(DashCoroutine());
        }
        else if (rand < 72)
        {
            StartCoroutine(HammerCoroutine());
        }
        else
        {
            StartCoroutine(ArrowCoroutine());
        }

    }


    protected void TeleportToSide()
    {

        float targetX = (playerLocationIntigger == 1) ? -11f : 11f;
        Vector3 currentScale = transform.localScale;

        if (playerLocationIntigger == 1)
        {
            currentScale.x = -1; 
        }

        else
        {
            currentScale.x = 1; 
        }
        transform.localScale = currentScale;

        transform.position = new Vector3(targetX, -3, transform.position.z);
    }

    protected IEnumerator TeleportComeBack()
    {
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(0.33f);
        OnSkillFinished();
    }
    protected void TeleportDown()
    {
        StartCoroutine(TeleportComeBack());
    }

    protected void TeleportToTopCenter()
    {

        float targetX = 0;

        transform.localScale = new Vector3(1, 1, 1);
        transform.position = transform.position = new Vector3(targetX, 7, transform.position.z);
    }
    protected void TeleportToPlayer()
    {
        isAttacking = true;
        isSkillCool = true;

        StartCoroutine(TeleportToPlayerCoroutine());
    }

    protected IEnumerator TeleportToPlayerCoroutine()
    {

        anim.SetBool("isWalking", false);
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f); 


        Vector3 targetPos;
        Vector3 targetScale;
        float targetY = transform.position.y; 


        if (playerPosition.x > 0) 
        {

            targetPos = new Vector3(playerPosition.x - 3, targetY, 0);

            targetScale = new Vector3(-1, 1, 1);
        }
        else 
        {
           
            targetPos = new Vector3(playerPosition.x + 3, targetY, 0);
          
            targetScale = new Vector3(1, 1, 1);
        }

        transform.position = targetPos;
        transform.localScale = targetScale;

      
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        OnSkillFinished();
    }

    protected IEnumerator HorizontalSlash()
    {
        isAttacking = true;
        isSkillCool = true;
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        anim.SetTrigger("Appear");
        TeleportToTopCenter();
        yield return new WaitForSeconds(0.33f);
        anim.SetTrigger("Hor");

    }
    protected IEnumerator VerticalSlashCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");

        yield return new WaitForSeconds(0.33f);
        TeleportToSide();

        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("Ver");

    }

    void VerSlash()
    {

        if (veriticalSlash == null)
        {
            Debug.Log("add");
        }
        int bossLocation;
        if (transform.position.x > 0)
        {
            bossLocation = 1;
        }
        else
        {
            bossLocation = -1;
        }
        GameObject verslash = Instantiate(veriticalSlash, new Vector3(bossLocation * 10, transform.position.y + 1, transform.position.y)
        , transform.rotation);
        verslash.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, false);

    }

    protected IEnumerator DashCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);

        Vector3 startPosition = transform.position;

        Vector3 endPosition = new Vector3(-startPosition.x, startPosition.y, startPosition.z);

        anim.SetTrigger("Dash");

        float elapsedTime = 0f;
        float duration = 0.2f;

        yield return new WaitForSeconds(0.66f);

        while (elapsedTime < duration)
        {

            float t = elapsedTime / duration;


            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }


        transform.position = endPosition;


    }
    protected IEnumerator LandingCoroutine()
    {

        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);


        Debug.Log(playerPosition.x);

        gameObject.transform.position = new Vector3(0, 12, 0);
        yield return new WaitForSeconds(2f);
        Vector3 startPosition = new Vector3(playerPosition.x, 12, 0);
        Vector3 endPosition = new Vector3(playerPosition.x, -3, 0);
        gameObject.transform.position = startPosition;




        anim.SetTrigger("Landing");

  
        float elapsedTime = 0f;
        float duration = 0.2f; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;


    }

    protected void Attack()
    {
        isAttacking = true;
        isBasicAttackCool = true;
        anim.SetTrigger("Attack");
    }
    protected IEnumerator HammerCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");

        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
   
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("Hammer");
    }
    void FindAndHammer()
    {
        if (playerPosition.x > 0)
        {
            int playerInt = 1;

            GameObject hammer = Instantiate(hammerEffect, new Vector3(playerPosition.x - 3, playerPosition.y + 0.2f, 0), Quaternion.identity);
            hammer.GetComponent<FinalBoss_Projectile>().Init(playerInt);
            Destroy(hammer, 1f);
        }
        else
        {
            int playerInt = -1;

            GameObject hammer = Instantiate(hammerEffect, new Vector3(playerPosition.x + 3, playerPosition.y + 0.2f, 0), Quaternion.identity);
            hammer.GetComponent<FinalBoss_Projectile>().Init(playerInt);
            Destroy(hammer, 1f);
        }
    }
    protected void ArrowToTop()
    {

    }
    protected IEnumerator ArrowCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;

        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("Arrow");

    }
    void ArrowEnable()
    {

        int landInt = UnityEngine.Random.Range(0, 2);

        if (BossPhase == 0 || landInt == 0)
        {
            Vector3 arrow00Location;
            Vector3 arrow01Location;
            Vector3 arrow02Location;
            int bossLocation;

            if (transform.position.x > 0)
            {
                bossLocation = 1;
                arrow00Location = new Vector3(9.5f, -3f, 0f);
                arrow01Location = new Vector3(9.3f, -3.5f, 0f);
                arrow02Location = new Vector3(9.5f, -4f, 0f);
            }
            else
            {
                bossLocation = -1;
                arrow00Location = new Vector3(-9.5f, -3f, 0f);
                arrow01Location = new Vector3(-9.3f, -3.5f, 0f);
                arrow02Location = new Vector3(-9.5f, -4f, 0f);
            }

            GameObject arrow00 = Instantiate(arrowEffect, arrow00Location, transform.rotation);
            arrow00.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, true);
            GameObject arrow01 = Instantiate(arrowEffect, arrow01Location, transform.rotation);
            arrow01.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, true);
            GameObject arrow02 = Instantiate(arrowEffect, arrow02Location, transform.rotation);
            arrow02.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, true);

        }
        else if (BossPhase == 1 && landInt == 1)
        {
            int numberOfArrows = 30;
            float minX = -12f;
            float maxX = 12f;
            float fixedY = 12f;

            for (int i = 0; i < numberOfArrows; i++)
            {
 
                float randomX = UnityEngine.Random.Range(minX, maxX);
                Vector3 spawnLocation = new Vector3(randomX, fixedY, 0f);

                GameObject arrow = Instantiate(arrowRainEffect, spawnLocation, Quaternion.identity); 

            }

            finalBossAudio.SoundPlay(7);

        }

    }


    public void OnBasicAttackFinished()
    {
        isAttacking = false; 
        isBasicAttackCool = true;

        Invoke("OnBasicAttackCooldownFinished", attackCooldown);
    }


    private void OnBasicAttackCooldownFinished()
    {
        isBasicAttackCool = false;
    }
    public void OnSkillFinished()
    {
        isAttacking = false; 


        Invoke("OnSkillCooldownFinished", skillCooldown);
    }

    private void OnSkillCooldownFinished()
    {
        isSkillCool = false;
    }
    


}
=======
using System.Collections;
using System.ComponentModel.Design;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FinalBossCombat : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected AnimatorConverter animatorConverter;
    protected UIManager uiManager;
    protected BoxCollider2D boxCollider;
    FinalBossAudio finalBossAudio;
    [SerializeField]
    protected Transform target;
    protected Vector3 playerPosition;
    private int playerLocationIntigger;
    bool isOpenning = true;

    public bool isAttacking;

    public bool isBasicAttackCool;
    public bool isSkillCool;
    [SerializeField]
    float attackCooldown = 1f;
    [SerializeField]
    float skillCooldown = 5f;

    [SerializeField]
    protected float bossMaxHP = 500;
    [SerializeField]
    protected float bossCurrentHP = 500;

    public int BossPhase;
    bool bossCleared = false;
    [SerializeField]
    GameObject dashEffect;
    [SerializeField]
    GameObject veriticalSlash;
    [SerializeField]
    GameObject horizontalSlash;
    [SerializeField]
    GameObject[] attackCol;
    [SerializeField]
    GameObject hammerEffect;
    [SerializeField]
    GameObject arrowEffect;
    [SerializeField]
    GameObject arrowRainEffect;

    public Slider bossHPSlider;
    [SerializeField]
    protected float moveSpeed = 2.5f;
    [SerializeField]
    protected float minDistanceToPlayer = 3f;
    [SerializeField]
    protected float maxDistanceToPlayer = 10f;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isAttacking = false;
        isBasicAttackCool = false;
        isSkillCool = false;

        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        animatorConverter = GameObject.Find("Player").GetComponent<AnimatorConverter>();

        InvokeRepeating("UpdatePosition", 0.05f, 0.5f);
        BossPhase = 0;


        InvokeRepeating("TrySkillPattern", 0.5f, 2.5f);
        BossPhase = 0;
        finalBossAudio = GetComponent<FinalBossAudio>();
    }


    void Update()
    {
        bossHPSlider.maxValue = bossMaxHP;
        bossHPSlider.value = bossCurrentHP;
        if (!bossCleared)
        {
            if (isAttacking || isOpenning)
            {
                anim.SetBool("isWalking", false);
                return; 
            }

            if (target == null)
            {
                anim.SetBool("isWalking", false);
                return;
            }


            HandleMovementAndFlip();
            if (bossCurrentHP <= bossMaxHP / 2)
            {
                BossPhase = 1;
                Debug.Log("bossPhase:" + BossPhase);
            }
        }

    }


    public void FinalBossCombatStart()
    {
        isOpenning = false;
        boxCollider.enabled = true;
        StartCoroutine(LandingCoroutine());
        Invoke("SliderActive", 2.5f);
    }
    public void SliderActive()
    {
        bossHPSlider.gameObject.SetActive(true);
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
        bossCleared = true;
        anim.SetTrigger("Die");
        boxCollider.enabled = false;
        bossHPSlider.gameObject.SetActive(false);

        StopAllCoroutines();

        CancelInvoke("UpdatePosition");
        CancelInvoke("TrySkillPattern");
        GameObject[] enemyAttack = GameObject.FindGameObjectsWithTag("BossAttack");

        foreach (GameObject Attack in enemyAttack)
        {

            Destroy(Attack);
        }
        StartCoroutine(FadeOutStart());
        Invoke("ClearSceneChange", uiManager.fadeDuration+4.5f);
    }
    IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(2f);
        uiManager.FadeOutStart();
    }
    void ClearSceneChange()
    {
        GameManager.Instance.playerInstanceCurrentHP = GameObject.Find("Player").GetComponent<PlayerCombat>().currentHP;
        GameManager.Instance.playerInstanceCurrentST = GameObject.Find("Player").GetComponent<PlayerCombat>().currentST;
        SceneManager.LoadScene("Stage10");
    }

    protected virtual void UpdatePosition()
    {

        if (target != null)
        {

            playerPosition = target.position;
            if (playerPosition.x >= 0)
            {
                playerLocationIntigger = 1;
            }
            else
            {
                playerLocationIntigger = -1;
            }

            if (isAttacking)
            {
                return;
            }


        }
    }
    protected void HandleMovementAndFlip()
    {

        if (!isOpenning)
        {

            Vector3 currentScale = transform.localScale;


            if (playerPosition.x > transform.position.x)
            {
                currentScale.x = -1; 
            }

            else if (playerPosition.x < transform.position.x)
            {
                currentScale.x = 1; 
            }
            transform.localScale = currentScale;

            float distanceX = Mathf.Abs(playerPosition.x - transform.position.x);
            if (distanceX > maxDistanceToPlayer && !isSkillCool)
            {
                TeleportToPlayer();
            }

            else if (distanceX > minDistanceToPlayer)
            {
                
                anim.SetBool("isWalking", true);

                Vector3 targetPosition = new Vector3(playerPosition.x, transform.position.y, transform.position.z);

                transform.position = Vector3.MoveTowards(
                    transform.position,    
                    targetPosition,        
                    moveSpeed * Time.deltaTime 
                );
            }

            else
            {

                anim.SetBool("isWalking", false);


                if (!isSkillCool)
                {

                    TrySkillPattern();
                }

                else if (!isBasicAttackCool)
                {
                    Attack();
                }
            }
        }
    }

    protected void TrySkillPattern()
    {
        
        if (isAttacking || isSkillCool || isOpenning)
        {
            return;
        }


        if (BossPhase == 0)
        {
            FirstPhasePatterCalculation();
        }
        else if (BossPhase == 1)
        {
            SecondPhasePaterenCalculation();
        }
    }


    protected void FirstPhasePatterCalculation()
    {

        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < 23)
        {

            StartCoroutine(LandingCoroutine());
        }

        else if (rand < 46)
        {
            StartCoroutine(HorizontalSlash()); 
        }

        else if (rand < 64)
        {

            StartCoroutine(VerticalSlashCoroutine());
        }

        else if (rand < 82)
        {

            StartCoroutine(ArrowCoroutine());
        }

        else
        {

            StartCoroutine(DashCoroutine());
        }
    }
    void SecondPhasePaterenCalculation()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 15)
        {
            StartCoroutine(LandingCoroutine());
        }
        else if (rand < 30)
        {
            StartCoroutine(HorizontalSlash());
        }
        else if (rand < 44)
        {
            StartCoroutine(VerticalSlashCoroutine());
        }
        else if (rand < 58)
        {
            StartCoroutine(DashCoroutine());
        }
        else if (rand < 72)
        {
            StartCoroutine(HammerCoroutine());
        }
        else
        {
            StartCoroutine(ArrowCoroutine());
        }

    }


    protected void TeleportToSide()
    {

        float targetX = (playerLocationIntigger == 1) ? -11f : 11f;
        Vector3 currentScale = transform.localScale;

        if (playerLocationIntigger == 1)
        {
            currentScale.x = -1; 
        }

        else
        {
            currentScale.x = 1; 
        }
        transform.localScale = currentScale;

        transform.position = new Vector3(targetX, -3, transform.position.z);
    }

    protected IEnumerator TeleportComeBack()
    {
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(0.33f);
        OnSkillFinished();
    }
    protected void TeleportDown()
    {
        StartCoroutine(TeleportComeBack());
    }

    protected void TeleportToTopCenter()
    {

        float targetX = 0;

        transform.localScale = new Vector3(1, 1, 1);
        transform.position = transform.position = new Vector3(targetX, 7, transform.position.z);
    }
    protected void TeleportToPlayer()
    {
        isAttacking = true;
        isSkillCool = true;

        StartCoroutine(TeleportToPlayerCoroutine());
    }

    protected IEnumerator TeleportToPlayerCoroutine()
    {

        anim.SetBool("isWalking", false);
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f); 


        Vector3 targetPos;
        Vector3 targetScale;
        float targetY = transform.position.y; 


        if (playerPosition.x > 0) 
        {

            targetPos = new Vector3(playerPosition.x - 3, targetY, 0);

            targetScale = new Vector3(-1, 1, 1);
        }
        else 
        {
           
            targetPos = new Vector3(playerPosition.x + 3, targetY, 0);
          
            targetScale = new Vector3(1, 1, 1);
        }

        transform.position = targetPos;
        transform.localScale = targetScale;

      
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        OnSkillFinished();
    }

    protected IEnumerator HorizontalSlash()
    {
        isAttacking = true;
        isSkillCool = true;
        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        anim.SetTrigger("Appear");
        TeleportToTopCenter();
        yield return new WaitForSeconds(0.33f);
        anim.SetTrigger("Hor");

    }
    protected IEnumerator VerticalSlashCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");

        yield return new WaitForSeconds(0.33f);
        TeleportToSide();

        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("Ver");

    }

    void VerSlash()
    {

        if (veriticalSlash == null)
        {
            Debug.Log("add");
        }
        int bossLocation;
        if (transform.position.x > 0)
        {
            bossLocation = 1;
        }
        else
        {
            bossLocation = -1;
        }
        GameObject verslash = Instantiate(veriticalSlash, new Vector3(bossLocation * 10, transform.position.y + 1, transform.position.y)
        , transform.rotation);
        verslash.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, false);

    }

    protected IEnumerator DashCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);

        Vector3 startPosition = transform.position;

        Vector3 endPosition = new Vector3(-startPosition.x, startPosition.y, startPosition.z);

        anim.SetTrigger("Dash");

        float elapsedTime = 0f;
        float duration = 0.2f;

        yield return new WaitForSeconds(0.66f);

        while (elapsedTime < duration)
        {

            float t = elapsedTime / duration;


            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }


        transform.position = endPosition;


    }
    protected IEnumerator LandingCoroutine()
    {

        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);


        Debug.Log(playerPosition.x);

        gameObject.transform.position = new Vector3(0, 12, 0);
        yield return new WaitForSeconds(2f);
        Vector3 startPosition = new Vector3(playerPosition.x, 12, 0);
        Vector3 endPosition = new Vector3(playerPosition.x, -3, 0);
        gameObject.transform.position = startPosition;




        anim.SetTrigger("Landing");

  
        float elapsedTime = 0f;
        float duration = 0.2f; 

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;


    }

    protected void Attack()
    {
        isAttacking = true;
        isBasicAttackCool = true;
        anim.SetTrigger("Attack");
    }
    protected IEnumerator HammerCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;


        anim.SetTrigger("Teleport");

        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
   
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("Hammer");
    }
    void FindAndHammer()
    {
        if (playerPosition.x > 0)
        {
            int playerInt = 1;

            GameObject hammer = Instantiate(hammerEffect, new Vector3(playerPosition.x - 3, playerPosition.y + 0.2f, 0), Quaternion.identity);
            hammer.GetComponent<FinalBoss_Projectile>().Init(playerInt);
            Destroy(hammer, 1f);
        }
        else
        {
            int playerInt = -1;

            GameObject hammer = Instantiate(hammerEffect, new Vector3(playerPosition.x + 3, playerPosition.y + 0.2f, 0), Quaternion.identity);
            hammer.GetComponent<FinalBoss_Projectile>().Init(playerInt);
            Destroy(hammer, 1f);
        }
    }
    protected void ArrowToTop()
    {

    }
    protected IEnumerator ArrowCoroutine()
    {
        isAttacking = true;
        isSkillCool = true;

        anim.SetTrigger("Teleport");
        yield return new WaitForSeconds(0.33f);
        TeleportToSide();
        anim.SetTrigger("Appear");
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("Arrow");

    }
    void ArrowEnable()
    {

        int landInt = UnityEngine.Random.Range(0, 2);

        if (BossPhase == 0 || landInt == 0)
        {
            Vector3 arrow00Location;
            Vector3 arrow01Location;
            Vector3 arrow02Location;
            int bossLocation;

            if (transform.position.x > 0)
            {
                bossLocation = 1;
                arrow00Location = new Vector3(9.5f, -3f, 0f);
                arrow01Location = new Vector3(9.3f, -3.5f, 0f);
                arrow02Location = new Vector3(9.5f, -4f, 0f);
            }
            else
            {
                bossLocation = -1;
                arrow00Location = new Vector3(-9.5f, -3f, 0f);
                arrow01Location = new Vector3(-9.3f, -3.5f, 0f);
                arrow02Location = new Vector3(-9.5f, -4f, 0f);
            }

            GameObject arrow00 = Instantiate(arrowEffect, arrow00Location, transform.rotation);
            arrow00.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, true);
            GameObject arrow01 = Instantiate(arrowEffect, arrow01Location, transform.rotation);
            arrow01.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, true);
            GameObject arrow02 = Instantiate(arrowEffect, arrow02Location, transform.rotation);
            arrow02.GetComponent<FinalBoss_Vertical>().Init(-bossLocation, true);

        }
        else if (BossPhase == 1 && landInt == 1)
        {
            int numberOfArrows = 30;
            float minX = -12f;
            float maxX = 12f;
            float fixedY = 12f;

            for (int i = 0; i < numberOfArrows; i++)
            {
 
                float randomX = UnityEngine.Random.Range(minX, maxX);
                Vector3 spawnLocation = new Vector3(randomX, fixedY, 0f);

                GameObject arrow = Instantiate(arrowRainEffect, spawnLocation, Quaternion.identity); 

            }

            finalBossAudio.SoundPlay(7);

        }

    }


    public void OnBasicAttackFinished()
    {
        isAttacking = false; 
        isBasicAttackCool = true;

        Invoke("OnBasicAttackCooldownFinished", attackCooldown);
    }


    private void OnBasicAttackCooldownFinished()
    {
        isBasicAttackCool = false;
    }
    public void OnSkillFinished()
    {
        isAttacking = false; 


        Invoke("OnSkillCooldownFinished", skillCooldown);
    }

    private void OnSkillCooldownFinished()
    {
        isSkillCool = false;
    }
    


}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
