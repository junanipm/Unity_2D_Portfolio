<<<<<<< HEAD
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDash : Enemy
{
    bool isDashing;
    bool isDashAble;
    protected override void Awake()
    {
        base.Awake();
        isDashing = false;
        isDashAble = true;
    }
    protected override void Start()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        animator.SetBool("isMoving", true);
        attackCol.enabled = false;
    }
    protected override void FixedUpdate()
    {
        if(isDashing || enemyDead ||enemyDamaged)
            return;

        if(!isDashing)
        {

            MovementChange();
        }
        if(!enemyDamaged || !isDashing)
            Move();
        

        
    }
    
    protected override void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";
        if (!isDashing)
        {
            if (movementFlag == 1) dist = "Left";
            else if (movementFlag == 2) dist = "Right";
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
        Vector2 newPosition = rigid.position + (Vector2)(moveVelocity * enemySpeed * Time.fixedDeltaTime);
        rigid.MovePosition(newPosition);
    }

    public override void OnFindTargetInRange(GameObject gameObject)
    {
        if(isDashAble && !enemyDead)
        {
            isDashing = true;
            animator.SetTrigger("EAttack");

            if(transform.position.x < gameObject.transform.position.x )
            {
                nextMove = 1;
                transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                nextMove = -1;
                transform.localScale = new Vector3(1,1,1);
            }
        
            StartCoroutine(EnemyDashControl()); 
            StartCoroutine(EnemyDashCoolDown());  
        }

    }
    IEnumerator EnemyDashControl() 
    {
        yield return new WaitForSeconds(0.5f);
        attackCol.enabled = true;
        rigid.linearVelocity = new Vector2(nextMove * 20f, rigid.linearVelocity.y);
        yield return new WaitForSeconds(0.3f);
        attackCol.enabled = false;
        rigid.linearVelocity = Vector2.zero;
        firstPoint.x = transform.position.x -5;
        secondPoint.x = transform.position.x + 5;
        isDashing = false;
    }
    IEnumerator EnemyDashCoolDown() 
    {
        isDashAble = false;
        yield return new WaitForSeconds(5f);
        isDashAble = true;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
            if(attackCol.enabled && other.CompareTag("Player"))
            {
                PlayerCombat player = other.GetComponent<PlayerCombat>();
                if(player != null)
                {
                    player.TakeDamage(gameObject.transform.position, enemyAttackP*2);
                }
                Debug.Log("TLQKF");
            }
        }
    }

    
}
=======
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDash : Enemy
{
    bool isDashing;
    bool isDashAble;
    protected override void Awake()
    {
        base.Awake();
        isDashing = false;
        isDashAble = true;
    }
    protected override void Start()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        animator.SetBool("isMoving", true);
        attackCol.enabled = false;
    }
    protected override void FixedUpdate()
    {
        if(isDashing || enemyDead ||enemyDamaged)
            return;

        if(!isDashing)
        {

            MovementChange();
        }
        if(!enemyDamaged || !isDashing)
            Move();
        

        
    }
    
    protected override void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";
        if (!isDashing)
        {
            if (movementFlag == 1) dist = "Left";
            else if (movementFlag == 2) dist = "Right";
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
        Vector2 newPosition = rigid.position + (Vector2)(moveVelocity * enemySpeed * Time.fixedDeltaTime);
        rigid.MovePosition(newPosition);
    }

    public override void OnFindTargetInRange(GameObject gameObject)
    {
        if(isDashAble && !enemyDead)
        {
            isDashing = true;
            animator.SetTrigger("EAttack");

            if(transform.position.x < gameObject.transform.position.x )
            {
                nextMove = 1;
                transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                nextMove = -1;
                transform.localScale = new Vector3(1,1,1);
            }
        
            StartCoroutine(EnemyDashControl()); 
            StartCoroutine(EnemyDashCoolDown());  
        }

    }
    IEnumerator EnemyDashControl() 
    {
        yield return new WaitForSeconds(0.5f);
        attackCol.enabled = true;
        rigid.linearVelocity = new Vector2(nextMove * 20f, rigid.linearVelocity.y);
        yield return new WaitForSeconds(0.3f);
        attackCol.enabled = false;
        rigid.linearVelocity = Vector2.zero;
        firstPoint.x = transform.position.x -5;
        secondPoint.x = transform.position.x + 5;
        isDashing = false;
    }
    IEnumerator EnemyDashCoolDown() 
    {
        isDashAble = false;
        yield return new WaitForSeconds(5f);
        isDashAble = true;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
            if(attackCol.enabled && other.CompareTag("Player"))
            {
                PlayerCombat player = other.GetComponent<PlayerCombat>();
                if(player != null)
                {
                    player.TakeDamage(gameObject.transform.position, enemyAttackP*2);
                }
                Debug.Log("TLQKF");
            }
        }
    }

    
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
