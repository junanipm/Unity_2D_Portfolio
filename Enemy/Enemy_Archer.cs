using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    [SerializeField] private float attackRange = 7f; 

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private float attackCooldown = 3f;
    public int enemyType = 0;
    private bool playerInRange;
    private bool isReloading;
    private int enemydirection;
    protected override void Awake()
    {
        base.Awake();
        playerInRange = false;
    }
    protected override void Start()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        animator.SetBool("isMoving", false);
    }
    protected override void FixedUpdate()
    {
        if(isAttacking || enemyDamaged || enemyDead) return;
        if (!isTracing)
        {
            if (!IsGroundAhead() && IsGrounded())
            {
                Debug.Log("낭떠러지 감지 → 방향 전환");
                ChangeDirection();
            }

            MovementChange();
        }
        if(!enemyDamaged)
            Move();
    }

    protected override void Update()
    {
        if(enemyDead) return;
        if(!hasAttacked && traceTarget != null)
        {
            float absDist = Mathf.Abs(rigid.position.x - traceTarget.transform.position.x);
            float distY = Mathf.Abs(rigid.position.y - traceTarget.transform.position.y);


            if (absDist <= attackRange && !isReloading && distY <= 1f)
            {
                Debug.Log("▶▶ 공격 조건 충족, 트리거 발동");
                playerInRange = true;
                StartCoroutine(ArcherAttack());
                Debug.Log("DSDAD");
            }
        }

    }
    protected override void Move()
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


            animator.SetBool("isMoving", false); 
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
          
        }
        if(dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1,1,1);
        }
        else if(dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1,1,1);
        }
        float speed = isTracing ? chasingSpeed : enemySpeed;
        if(playerInRange) chasingSpeed = 0;
        Vector2 newPosition = rigid.position + (Vector2)(moveVelocity*speed*Time.fixedDeltaTime);
        rigid.MovePosition(newPosition);
    }
    
    protected override IEnumerator ChangeMovement(int wayPoint)
    {
        Debug.Log("sss");
        isChanging = true;
        movementFlag = 0;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(2f);
        isChanging = false;
        yield return new WaitForSeconds(0.5f);
        movementFlag = wayPoint;
        if(playerInRange)
        {
            animator.SetBool("isMoving", false);
            Debug.Log("이건또왜됨");
        }
            
        else if(!playerInRange)
        {
            animator.SetBool("isMoving", true);
            Debug.Log("이거왜안됨");
        }
            
    
    }


    protected IEnumerator ArcherAttack()
    {
        animator.SetBool("isMoving", false);

        animator.SetTrigger("EnemyAttack");
        isReloading = true;
        
        yield return new WaitForSeconds(attackCooldown);
        isReloading = false;
    }
    public override void OnTargetLost()
    {
        if (isTracing)
        {
            Debug.Log("lost");
            isTracing = false;
            traceTarget = null;
            playerInRange = false;
            animator.SetBool("isMoving", true);
            firstPoint.x = transform.position.x - 5;
            secondPoint.x = transform.position.x + 5;
            StartCoroutine(ChangeMovement(1));
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

        if (isAttacking)
        {
            StopCoroutine("EnemyAttackCool"); 
            animator.ResetTrigger("EnemyAttack"); 
            animator.Play("Enemy_Archer_Idle"); 
            isAttacking = false;
            hasAttacked = false;
            attackCol.enabled = false;
        }


        rigid.linearVelocity = new Vector2(playerVec * distance, 10);

        yield return new WaitForSeconds(0.4f);

        enemyDamaged = false;
    }
    public override void OnFindTargetInRange(GameObject gameObject)
    {
        var player = gameObject.GetComponent<PlayerCombat>();
        if(player != null)
        {
            if(!isTracing)
            {
                isTracing = true;
                
                traceTarget = gameObject;
                
                if(!playerInRange)
                {
                    chasingSpeed = 1;
                    animator.SetBool("isMoving", true);
                }
                
                if(transform.position.x < gameObject.transform.position.x )
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
    void MeleeAttack()
    {
        float fireDifference;
        if (enemyType == 0)
        {
            fireDifference = 0.2f;
        }
        else
        {
            fireDifference = 0;
        }
        Vector3 realFirePoint =new Vector3 (firePoint.position.x, firePoint.position.y -fireDifference, firePoint.position.z);
        GameObject arrow = Instantiate(arrowPrefab, realFirePoint, transform.rotation);
        arrow.GetComponent<EnemyProjectile>().Init(1.5f*enemyAttackP, -1*gameObject.transform.localScale.x*firePoint.right);
    }

}