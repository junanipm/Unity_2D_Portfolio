using System.Collections;
using UnityEngine;

public class Yellow_Boss_Sphere : Yellow_Boss_Projectile
{
    Transform target;
    public float speed = 5f;
    public float lifeTime = 3f;

    [SerializeField]
    GameObject explosion;
    public AudioClip audioClip;

 
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
        StartCoroutine(SelfDestroy());
        

    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null && !hasHit)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
                hasHit = true;

                SoundManager.instance.SFXPlay("Explode", audioClip);
                GameObject spExplosion = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                StopCoroutine(SelfDestroy());
                Destroy(spExplosion, 1f);
                Destroy(gameObject);
            }
        }

    }
    IEnumerator SelfDestroy()
    {
        if (!hasHit)
        {
            yield return new WaitForSeconds(lifeTime);
            SoundManager.instance.SFXPlay("Explode", audioClip);
            GameObject spExplosion = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(spExplosion, 1f);
            Destroy(gameObject);
            yield break;
        }
        if (hasHit)
        {
            yield return null;
        }
    }
}
