<<<<<<< HEAD
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25f;
    public float lifeTime = 2f;

    private float damage;
    private bool hasHit = false;
    private Vector2 direction;

    public GameObject hitEffect;
    public void Init(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        if(direction.x <0)
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

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyOnDamaged(damage, 3);
            }

            Debug.Log("ArrowHit");
            Vector3 hitPosition = other.ClosestPoint(transform.position);
                          if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            hasHit = true;
            YellowBossCombat yellowBossCombat = other.GetComponent<YellowBossCombat>();
            if (yellowBossCombat != null)
            {
                yellowBossCombat.OnDamaged(damage);
            }
            FinalBossCombat finalBossCombat = other.GetComponent<FinalBossCombat>();
            if (finalBossCombat != null)
            {
                finalBossCombat.OnDamaged(damage);
            }

            Debug.Log("ArrowHit");
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            
           
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }
            Destroy(gameObject);
        }
    }
=======
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25f;
    public float lifeTime = 2f;

    private float damage;
    private bool hasHit = false;
    private Vector2 direction;

    public GameObject hitEffect;
    public void Init(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        if(direction.x <0)
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

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyOnDamaged(damage, 3);
            }

            Debug.Log("ArrowHit");
            Vector3 hitPosition = other.ClosestPoint(transform.position);
                          if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            hasHit = true;
            YellowBossCombat yellowBossCombat = other.GetComponent<YellowBossCombat>();
            if (yellowBossCombat != null)
            {
                yellowBossCombat.OnDamaged(damage);
            }
            FinalBossCombat finalBossCombat = other.GetComponent<FinalBossCombat>();
            if (finalBossCombat != null)
            {
                finalBossCombat.OnDamaged(damage);
            }

            Debug.Log("ArrowHit");
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            
           
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }
            Destroy(gameObject);
        }
    }
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
}