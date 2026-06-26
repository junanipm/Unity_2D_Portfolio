using System.Collections;
using UnityEngine;

public class FinalBoss_ArrowRain : FinalBoss_Projectile
{
 
    public float speed;

    Animator animator;
    CapsuleCollider2D capCol;

    bool isDelayed =false;
    float targetLandY;
    bool isLanded;
    void Start()
    {
        animator = GetComponent<Animator>();
        capCol = GetComponent<CapsuleCollider2D>();

        targetLandY = UnityEngine.Random.Range(-5.1f, -4.3f);

        isLanded = false;
        isDelayed = true;
        cor();
    }

    void Update()
    {
        if (!isDelayed && !isLanded) transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < targetLandY && !isLanded)
        {

            transform.position = new Vector3(transform.position.x, targetLandY, transform.position.z);
            
            arrowLanding();
        }
    }
    void cor()
    {
        StartCoroutine(DelayCoroutine());
    }
    void arrowLanding()
    {

        isLanded = true;

        capCol.enabled = false;

        int landingInt = UnityEngine.Random.Range(0, 2);
        if (landingInt == 0)
        {
            animator.SetTrigger("Landing00");
        }
        else
        {
            animator.SetTrigger("Landing01");
        }


        Destroy(gameObject, 1.5f);
    }

    protected IEnumerator DelayCoroutine()
    {
        float rand;

        rand = UnityEngine.Random.Range(0.5f, 4.5f);
        yield return new WaitForSeconds(rand);
        isDelayed = false;
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null && !hasHit)
            {
                hasHit = true;
                playerCombat.TakeDamage(gameObject.transform.position, damage);

                Debug.Log("playerHit");
            }

        }
        
    }
}
