<<<<<<< HEAD
using System.Collections;
using UnityEngine;

public class YellowSkill : MonoBehaviour
{
    public GameObject hitEffect;
    private float damage;
    private bool hasHit = false;
    private float hitTurm;
    private bool isCooldown = false;

    public void Init(float damage, float hitTurm)
    {
        this.damage = 0.5f* damage;
        this.hitTurm = hitTurm;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasHit)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            hasHit = true;


            Vector3 hitPosition = collision.ClosestPoint(transform.position);
            if (enemy != null)
            {
                enemy.EnemyOnDamaged(damage, 3);
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }
            StartCoroutine(hitTurmCheck());
        }
        else if (collision.CompareTag("Boss") && !hasHit)
        {
            FinalBossCombat finalBossCombat = collision.GetComponent<FinalBossCombat>();
            YellowBossCombat yellowBossCombat = collision.GetComponent<YellowBossCombat>();
            hasHit = true;
            

            Vector3 hitPosition = collision.ClosestPoint(transform.position);
            if (yellowBossCombat != null)
            {
                yellowBossCombat.OnDamaged(damage);
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }

            if (finalBossCombat != null)
            {
                finalBossCombat.OnDamaged(damage);
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }

            StartCoroutine(hitTurmCheck());
        }
    }
    IEnumerator hitTurmCheck()
    {
        if(hasHit)
        {
            yield return new WaitForSeconds(hitTurm);
            hasHit = false;   
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
=======
using System.Collections;
using UnityEngine;

public class YellowSkill : MonoBehaviour
{
    public GameObject hitEffect;
    private float damage;
    private bool hasHit = false;
    private float hitTurm;
    private bool isCooldown = false;

    public void Init(float damage, float hitTurm)
    {
        this.damage = 0.5f* damage;
        this.hitTurm = hitTurm;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasHit)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            hasHit = true;


            Vector3 hitPosition = collision.ClosestPoint(transform.position);
            if (enemy != null)
            {
                enemy.EnemyOnDamaged(damage, 3);
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }
            StartCoroutine(hitTurmCheck());
        }
        else if (collision.CompareTag("Boss") && !hasHit)
        {
            FinalBossCombat finalBossCombat = collision.GetComponent<FinalBossCombat>();
            YellowBossCombat yellowBossCombat = collision.GetComponent<YellowBossCombat>();
            hasHit = true;
            

            Vector3 hitPosition = collision.ClosestPoint(transform.position);
            if (yellowBossCombat != null)
            {
                yellowBossCombat.OnDamaged(damage);
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }

            if (finalBossCombat != null)
            {
                finalBossCombat.OnDamaged(damage);
            }
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
                Destroy(effect, 0.32f);
            }

            StartCoroutine(hitTurmCheck());
        }
    }
    IEnumerator hitTurmCheck()
    {
        if(hasHit)
        {
            yield return new WaitForSeconds(hitTurm);
            hasHit = false;   
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
