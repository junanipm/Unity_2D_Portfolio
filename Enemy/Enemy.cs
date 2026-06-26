using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;
public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected EnemyEffect enemyEffect;
    public int movementFlag = 1; 
    public PlayerCombat playerCombat;
    public Collider2D attackCol;
    public Vector3 firstPoint;
    public Vector3 secondPoint;
    protected GameObject traceTarget;
    protected int nextMove;
    [SerializeField]
    protected float enemySpeed = 1f;
    [SerializeField]
    protected float chasingSpeed = 3f;
    [SerializeField]
    protected float enemyLife = 100;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundDistance = 1.5f;
    [SerializeField]
    protected float groundCheckDistance = 0.1f;

    [SerializeField]
    protected float stopChasingDistance = 0.2f;
    [SerializeField]
    protected float maxTraceYDistance = 3.0f;

    protected bool enemyDamaged = false;

    protected bool enemyDead = false;

    protected bool isChanging = false;
    protected bool isTracing = false;
    protected bool isAttacking;
    protected bool hasAttacked = false;
    public int enemyAttackP = 10;
    [Header("오디오")]
    public AudioClip audioClip;

    

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyEffect = GetComponentInChildren<EnemyEffect>();
        firstPoint.y = gameObject.transform.position.y;
        secondPoint.y = gameObject.transform.position.y;
        firstPoint.x = gameObject.transform.position.x - 5;
        secondPoint.x = gameObject.transform.position.x + 5;
    }
    protected virtual void Start()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        animator.SetBool("isMoving", true);
        attackCol.enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        if (isAttacking || enemyDamaged || enemyDead) return;
        Move();
        if (!isTracing)
        {
            if (IsGrounded() && !IsGroundAhead())
            {
                Debug.Log("낭떠러지 감지 → 방향 전환");
                ChangeDirection();
            }

            MovementChange();
        }


    }
    protected virtual void Update()
    {
        if (enemyDead) return;

        if (!hasAttacked && traceTarget != null)
        {
            float absDist = Mathf.Abs(rigid.position.x - traceTarget.transform.position.x);
            float distY = Mathf.Abs(rigid.position.y - traceTarget.transform.position.y);
            if (absDist <= 1.5f && distY <= 1f)
            {
                animator.SetTrigger("EnemyAttack");
            }
        }



    }
    protected virtual void MovementChange()
    {
        if (isChanging) return;

        

        if (MathF.Abs(transform.position.x - firstPoint.x) <= 0.1f)
        {
            StartCoroutine(ChangeMovement(2));
        }
        else if (MathF.Abs(transform.position.x - secondPoint.x) <= 0.1f)
        {
            StartCoroutine(ChangeMovement(1));
        }
    }
    protected virtual void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";
        
        bool shouldMove = false; 

        if (isTracing && traceTarget != null)
        {
            Vector3 playerPos = traceTarget.transform.position;
            
            float distanceToPlayerX = Mathf.Abs(transform.position.x - playerPos.x);

            float distanceToPlayerY = Mathf.Abs(transform.position.y - playerPos.y);

            if (distanceToPlayerY <= maxTraceYDistance && distanceToPlayerX > stopChasingDistance)
            {
                shouldMove = true; 
                dist = (playerPos.x < transform.position.x) ? "Left" : "Right";
            }

            animator.SetBool("isRunning", shouldMove);
            animator.SetBool("isMoving", shouldMove); 
        }
        else 
        {
            if (movementFlag == 1)
            {
                dist = "Left";
                shouldMove = true;
            }
            else if (movementFlag == 2)
            {
                dist = "Right";
                shouldMove = true;
            }
            animator.SetBool("isMoving", shouldMove);
            animator.SetBool("isRunning", false);
        }


        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        float speed = isTracing ? chasingSpeed : enemySpeed;
        Vector2 newPosition = rigid.position + (Vector2)(moveVelocity * speed * Time.fixedDeltaTime);
        rigid.MovePosition(newPosition);
    }

    protected virtual IEnumerator ChangeMovement(int wayPoint)
    {

        isChanging = true;
        movementFlag = 0;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(2f);
        isChanging = false;
        yield return new WaitForSeconds(0.5f);
        movementFlag = wayPoint;

        animator.SetBool("isMoving", true);
    }

    protected virtual bool IsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.blue);
        return hit.collider != null;
    }

    protected virtual bool IsGroundAhead()
    {
        float directionX = (movementFlag == 1) ? -1f : 1f;
        Vector2 origin = new Vector2(transform.position.x + directionX * 0.5f, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundDistance, groundLayer);
        Debug.DrawRay(origin, Vector2.down * groundDistance, Color.red);

        return hit.collider != null;
    }
    protected virtual void ChangeDirection()
    {
        if (movementFlag == 1) movementFlag = 2;
        else if (movementFlag == 2) movementFlag = 1;
    }

    private Coroutine damageEffectCoroutine;
    public virtual void EnemyOnDamaged(float damage, float distance)
    {

        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine); 

        damageEffectCoroutine = StartCoroutine(DamageEffect(damage, distance));
        animator.SetTrigger("EnemyHit");

        if (enemyLife <= 0)
        {
            EnemyDieTrigger();
        }
    }
    protected virtual IEnumerator DamageEffect(float dmg, float distance)
    {
        AnimatorConverter playerMode = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorConverter>();

        float playerVec = 0;
        if (isTracing && traceTarget != null)
        {
            if (gameObject.transform.position.x - traceTarget.transform.position.x > 0)
            {
                playerVec = 1;
            }
            else if (gameObject.transform.position.x - traceTarget.transform.position.x < 0)
            {
                playerVec = -1;
            }
        }

        enemyLife -= dmg;
        enemyDamaged = true;

        if (isAttacking)
        {
            StopCoroutine("EnemyAttackCool"); 
            animator.ResetTrigger("EnemyAttack"); 

            animator.Play("Knight_Idle_Anim"); 
            isAttacking = false;
            hasAttacked = false;
            attackCol.enabled = false;
        }


        rigid.linearVelocity = new Vector2(playerVec * distance, 10);
  
        yield return new WaitForSeconds(0.5f);

        enemyDamaged = false;
    }
    protected virtual void EnemyDieTrigger()
    {
        if (enemyLife <= 0)
        {
            StartCoroutine(EnemyDie());
            enemyDead = true;
        }
    }
    protected virtual IEnumerator EnemyDie()
    {
        animator.SetTrigger("EnemyDie");
        gameObject.layer = 11;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }

    public virtual void OnFindTargetInRange(GameObject gameObject)
    {

        var player = gameObject.GetComponent<PlayerCombat>();
        if (player != null)
        {
            if (!isTracing && Mathf.Abs(rigid.position.y - player.transform.position.y) < 1f)
            {
                isTracing = true;
                traceTarget = gameObject;
                animator.SetBool("isRunning", true);


                if (transform.position.x < gameObject.transform.position.x)
                {
                    nextMove = 1;
                }
                else
                {
                    nextMove = -1;
                }


            }
        }
    }

    public virtual void OnTargetLost()
    {
        if (isTracing )
        {
            isTracing = false;
            traceTarget = null;
            animator.SetBool("isRunning", false);
            firstPoint.x = transform.position.x - 5;
            secondPoint.x = transform.position.x + 5;
            StartCoroutine(ChangeMovement(1));
        }
    }

    protected virtual void EnemyAttackReady()
    {
        isAttacking = true;
        StartCoroutine(EnemyAttackCool());
    }
    protected virtual void EnemyAttack()
    {
        attackCol.enabled = true;

    }

    protected virtual void EnemyAttackEnd()
    {
        attackCol.enabled = false;
        isAttacking = false;

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (attackCol.enabled && other.CompareTag("Player"))
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(gameObject.transform.position, enemyAttackP * 2);
            }
            Debug.Log("TLQKF");
        }
    }
    protected virtual IEnumerator EnemyAttackCool()
    {
        var WaitForSeconds = new WaitForSeconds(2f);
        hasAttacked = true;
        yield return WaitForSeconds;
        hasAttacked = false;
    }
    public virtual void EnemySoundPlay()
    {
        SoundManager.instance.SFXPlay("Attack", audioClip);
    }
    
}