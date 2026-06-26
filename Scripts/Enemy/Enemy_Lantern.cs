<<<<<<< HEAD
using System.Collections;
using UnityEngine;

public class Enemy_Lantern : Enemy
{
    public AudioClip audioClip2;
    protected bool isFireAttacking;
    protected bool hasFireAttacked;
    public Collider2D fireCol;
    
    protected override void Awake()
    {
        base.Awake();
        isFireAttacking = false;
        hasFireAttacked = false;
    }
    protected override void Start()
    {
        base.Start();
        fireCol.enabled = false;
    }
    protected override void FixedUpdate()
    {
        if(isAttacking || enemyDamaged ||isFireAttacking ||enemyDead) return;
        if (!isTracing)
        {

            if (!IsGroundAhead() &&IsGrounded())
            {
                Debug.Log("낭떠러지 감지 → 방향 전환");
                ChangeDirection();
            }

            MovementChange();
        }
        if(!enemyDamaged || !isAttacking ||!isFireAttacking)
            Move();
    }
    protected override void Update()
    {   if(enemyDead) return;
        if(!hasAttacked && traceTarget != null && !enemyDamaged)
        {
            float absDist = Mathf.Abs(rigid.position.x - traceTarget.transform.position.x);
            float distY = Mathf.Abs(rigid.position.y - traceTarget.transform.position.y);


            if (absDist <= 1.5f && !isFireAttacking && distY <= 1f)
            {

                animator.SetTrigger("EnemyAttack");
                animator.SetBool("EnemyFire", false);
            }
            else if (absDist > 1.5f && absDist <= 3f && !hasFireAttacked && distY <= 1f)
            {
                animator.SetTrigger("EnemyAttack");
                animator.SetBool("EnemyFire", true);

            }
        }
        
    }
    protected override IEnumerator DamageEffect(float dmg, float distance)
    {
        AnimatorConverter playerMode = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorConverter>();
        
        float playerVec = 0;
        if(isTracing && traceTarget != null)
        {
            if(gameObject.transform.position.x-traceTarget.transform.position.x > 0)
            {
                playerVec = 1;
            }
            else if(gameObject.transform.position.x-traceTarget.transform.position.x < 0)
            {
                playerVec = -1;
            }
        }
        

        enemyLife -= dmg;
        enemyDamaged = true;
        
        if (isAttacking || isFireAttacking)
        {
            StopCoroutine("EnemyAttackCool"); 
            StopCoroutine("EnemyFireattackCool");
            animator.ResetTrigger("EnemyAttack");
            animator.SetBool("EnemyFire", false);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Lantern_Attack_Anim") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Lantern_FAttack_Anim"))
            {
                animator.Play("Lantern_Idle_Anim", 0, 0f);
            }
            animator.Rebind();
            isAttacking = false;
            hasAttacked = false;
            isFireAttacking = false;
            fireCol.enabled = false;
            attackCol.enabled = false;
            
        }
        

        spriteRenderer.color = Color.red;

        rigid.linearVelocity = new Vector2(playerVec * distance, 10);

        yield return new WaitForSeconds(0.5f);
        
        spriteRenderer.color = Color.white;
        enemyDamaged = false;
    }
    public override void OnTargetLost()
    {
        if (isTracing)
        {
            isTracing = false;
            traceTarget = null;
 
            firstPoint.x = transform.position.x - 5;
            secondPoint.x = transform.position.x + 5;
            StartCoroutine(ChangeMovement(1));
        }
    }
    protected override void EnemyAttackReady()
    {
        if(isFireAttacking)
        {
            
            isFireAttacking = false;
            fireCol.enabled = false;
            StopCoroutine(EnemyFireattackCool());
            StopCoroutine(EnemyAttackCool());
        }
        isAttacking = true;
        StartCoroutine(EnemyAttackCool());
    }
    protected override void EnemyAttack()
    {
        if(isFireAttacking)
        {
            fireCol.enabled = true;
        }
        else
        {
            attackCol.enabled = true;
        }

    }
    protected override void EnemyAttackEnd()
    {
        attackCol.enabled = false;
        isAttacking = false;
        
    }

    protected void EnemyFireAttackReady()
    {
        attackCol.enabled = false;
        isFireAttacking = true;
        isAttacking = true;
        StartCoroutine(EnemyFireattackCool());
        StartCoroutine(EnemyAttackCool());
    }
    protected void EnemyFireAttackEnd()
    {
        fireCol.enabled = false;
        isAttacking = false;
        isFireAttacking = false;
    }
    protected IEnumerator EnemyFireattackCool()
    {
        var WaitForSeconds = new WaitForSeconds(4.3f);
        
        hasFireAttacked = true;
        yield return WaitForSeconds;
        hasFireAttacked = false;
        
    }
    protected override IEnumerator EnemyAttackCool()
    {
        var WaitForSeconds = new WaitForSeconds(2f);
        hasAttacked = true;
        yield return WaitForSeconds;
        hasAttacked = false;
    }
    public LayerMask playerLayer;
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (attackCol.enabled && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(gameObject.transform.position, enemyAttackP * 1.5f);
            }
            Debug.Log("TLQKF");
        }
        else if (fireCol.enabled && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(gameObject.transform.position, enemyAttackP * 3f);
            }
            Debug.Log("TLQKF2");
        }
    }

    public virtual void EnemySoundPlay2()
    {
        SoundManager.instance.SFXPlay("Attack2", audioClip2);
    }
}
=======
using System.Collections;
using UnityEngine;

public class Enemy_Lantern : Enemy
{
    public AudioClip audioClip2;
    protected bool isFireAttacking;
    protected bool hasFireAttacked;
    public Collider2D fireCol;
    
    protected override void Awake()
    {
        base.Awake();
        isFireAttacking = false;
        hasFireAttacked = false;
    }
    protected override void Start()
    {
        base.Start();
        fireCol.enabled = false;
    }
    protected override void FixedUpdate()
    {
        if(isAttacking || enemyDamaged ||isFireAttacking ||enemyDead) return;
        if (!isTracing)
        {

            if (!IsGroundAhead() &&IsGrounded())
            {
                Debug.Log("낭떠러지 감지 → 방향 전환");
                ChangeDirection();
            }

            MovementChange();
        }
        if(!enemyDamaged || !isAttacking ||!isFireAttacking)
            Move();
    }
    protected override void Update()
    {   if(enemyDead) return;
        if(!hasAttacked && traceTarget != null && !enemyDamaged)
        {
            float absDist = Mathf.Abs(rigid.position.x - traceTarget.transform.position.x);
            float distY = Mathf.Abs(rigid.position.y - traceTarget.transform.position.y);


            if (absDist <= 1.5f && !isFireAttacking && distY <= 1f)
            {

                animator.SetTrigger("EnemyAttack");
                animator.SetBool("EnemyFire", false);
            }
            else if (absDist > 1.5f && absDist <= 3f && !hasFireAttacked && distY <= 1f)
            {
                animator.SetTrigger("EnemyAttack");
                animator.SetBool("EnemyFire", true);

            }
        }
        
    }
    protected override IEnumerator DamageEffect(float dmg, float distance)
    {
        AnimatorConverter playerMode = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorConverter>();
        
        float playerVec = 0;
        if(isTracing && traceTarget != null)
        {
            if(gameObject.transform.position.x-traceTarget.transform.position.x > 0)
            {
                playerVec = 1;
            }
            else if(gameObject.transform.position.x-traceTarget.transform.position.x < 0)
            {
                playerVec = -1;
            }
        }
        

        enemyLife -= dmg;
        enemyDamaged = true;
        
        if (isAttacking || isFireAttacking)
        {
            StopCoroutine("EnemyAttackCool"); 
            StopCoroutine("EnemyFireattackCool");
            animator.ResetTrigger("EnemyAttack");
            animator.SetBool("EnemyFire", false);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Lantern_Attack_Anim") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Lantern_FAttack_Anim"))
            {
                animator.Play("Lantern_Idle_Anim", 0, 0f);
            }
            animator.Rebind();
            isAttacking = false;
            hasAttacked = false;
            isFireAttacking = false;
            fireCol.enabled = false;
            attackCol.enabled = false;
            
        }
        

        spriteRenderer.color = Color.red;

        rigid.linearVelocity = new Vector2(playerVec * distance, 10);

        yield return new WaitForSeconds(0.5f);
        
        spriteRenderer.color = Color.white;
        enemyDamaged = false;
    }
    public override void OnTargetLost()
    {
        if (isTracing)
        {
            isTracing = false;
            traceTarget = null;
 
            firstPoint.x = transform.position.x - 5;
            secondPoint.x = transform.position.x + 5;
            StartCoroutine(ChangeMovement(1));
        }
    }
    protected override void EnemyAttackReady()
    {
        if(isFireAttacking)
        {
            
            isFireAttacking = false;
            fireCol.enabled = false;
            StopCoroutine(EnemyFireattackCool());
            StopCoroutine(EnemyAttackCool());
        }
        isAttacking = true;
        StartCoroutine(EnemyAttackCool());
    }
    protected override void EnemyAttack()
    {
        if(isFireAttacking)
        {
            fireCol.enabled = true;
        }
        else
        {
            attackCol.enabled = true;
        }

    }
    protected override void EnemyAttackEnd()
    {
        attackCol.enabled = false;
        isAttacking = false;
        
    }

    protected void EnemyFireAttackReady()
    {
        attackCol.enabled = false;
        isFireAttacking = true;
        isAttacking = true;
        StartCoroutine(EnemyFireattackCool());
        StartCoroutine(EnemyAttackCool());
    }
    protected void EnemyFireAttackEnd()
    {
        fireCol.enabled = false;
        isAttacking = false;
        isFireAttacking = false;
    }
    protected IEnumerator EnemyFireattackCool()
    {
        var WaitForSeconds = new WaitForSeconds(4.3f);
        
        hasFireAttacked = true;
        yield return WaitForSeconds;
        hasFireAttacked = false;
        
    }
    protected override IEnumerator EnemyAttackCool()
    {
        var WaitForSeconds = new WaitForSeconds(2f);
        hasAttacked = true;
        yield return WaitForSeconds;
        hasAttacked = false;
    }
    public LayerMask playerLayer;
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (attackCol.enabled && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(gameObject.transform.position, enemyAttackP * 1.5f);
            }
            Debug.Log("TLQKF");
        }
        else if (fireCol.enabled && (playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(gameObject.transform.position, enemyAttackP * 3f);
            }
            Debug.Log("TLQKF2");
        }
    }

    public virtual void EnemySoundPlay2()
    {
        SoundManager.instance.SFXPlay("Attack2", audioClip2);
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
