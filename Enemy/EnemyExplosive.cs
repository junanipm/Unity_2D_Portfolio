
using Unity.VisualScripting;
using UnityEngine;

public class EnemyExplosive : MonoBehaviour
{
    
    private bool isDashing;
    private int movementFlag = 1;
    public int enemyAttackP = 20;
    public float enemySpeed = 2f;
    private bool hasDashed = false;

    Rigidbody2D rigid;
    Animator anim;
    Vector2 firstPoint;
    Vector2 secondPoint;
    Vector2 targetPoint;
    [SerializeField]
    GameObject explosionPrefab;
    float originalY;

    private float waveAmplitude = 0.5f;
    private float waveFrequency = 3f;
    private float waveTimer;

    void Start()
    {
       
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        float x = transform.position.x;
        float y = transform.position.y;
        firstPoint = new Vector2(x -3.5f, y);
        secondPoint = new Vector2(x + 3.5f, y);
        originalY = y;
        
    }

    void FixedUpdate()
    {
        if(isDashing) TraceTarget();
        else Move();
    }

    private void Move()
    {
        if(isDashing) return;
        waveTimer += Time.fixedDeltaTime;

        Vector3 moveVelocity = Vector3.zero;

        
        if (movementFlag == 1) 
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movementFlag == 2) 
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        Vector2 newPosition = rigid.position + (Vector2)(moveVelocity*enemySpeed*Time.fixedDeltaTime);
        newPosition.y = originalY + Mathf.Sin(waveTimer*waveFrequency)*waveAmplitude;
        rigid.MovePosition(newPosition);

        if (movementFlag == 1 && newPosition.x <= firstPoint.x)
        {
            movementFlag = 2;
        }
        else if (movementFlag == 2 && newPosition.x >= secondPoint.x)
        {
            movementFlag = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isDashing)
        {
            
            isDashing = true;
            enemySpeed = 5;
            
            targetPoint = collision.transform.position;
            
        }
    }
    void TraceTarget()
    {
        anim.SetTrigger("Attack");
        
        Vector2 currentPosition = rigid.position;
        Vector2 direction = (targetPoint - currentPosition).normalized;

        float scaleX = targetPoint.x < currentPosition.x ? 1: -1;
        float scaleY = targetPoint.y < currentPosition.y ? 1: -1;
        
        transform.localScale = new Vector3(scaleX, scaleY, 1);

        Vector2 nextPosition = currentPosition + direction * enemySpeed*2 * Time.deltaTime;
        rigid.MovePosition(nextPosition);

        if (Vector2.Distance(nextPosition, targetPoint) < 0.1f)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explosion.GetComponent<Explosion>().Init(enemyAttackP);
            
            Destroy(gameObject);
            
        }
    }
}
