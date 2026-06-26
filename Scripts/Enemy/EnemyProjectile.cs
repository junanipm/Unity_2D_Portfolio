<<<<<<< HEAD
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
 
    public float speed = 25f;
    public float lifeTime = 2f;

    protected float damage;
    protected bool hasHit = false;
    protected Vector2 direction;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if(playerCombat != null)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
                Destroy(gameObject);
            }
        }
    }
    protected void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public virtual void Init(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        if(direction.x >0)
        {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        Destroy(gameObject, lifeTime);
    }

}
=======
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
 
    public float speed = 25f;
    public float lifeTime = 2f;

    protected float damage;
    protected bool hasHit = false;
    protected Vector2 direction;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if(playerCombat != null)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
                Destroy(gameObject);
            }
        }
    }
    protected void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public virtual void Init(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        if(direction.x >0)
        {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        Destroy(gameObject, lifeTime);
    }

}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
