
using System.Collections;
using UnityEngine;

public class FinalBoss_Vertical : FinalBoss_Projectile
{

    public float speed;
    public float lifeTime;

    private Vector2 direction;

    bool isDelayed;
    public void Init(int direction, bool isdelay)
    {
        isDelayed = isdelay;
        if (direction < 0)
        {
            this.direction = Vector2.left;

            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            this.direction = Vector2.right;

            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
            
        if (!isDelayed)
        {

            Destroy(gameObject, lifeTime);
        }
        else
        {
            StartCoroutine(DelayCoroutine());
        }
        
    }
    protected IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        isDelayed = false;
    }

    protected virtual void Update()
    {
        if (!isDelayed) transform.Translate(direction * speed * Time.deltaTime);

    }
 
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);

                Debug.Log("playerHit");
            }

        }
    }
}
