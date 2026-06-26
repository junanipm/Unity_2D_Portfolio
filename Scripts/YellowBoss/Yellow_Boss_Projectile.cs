<<<<<<< HEAD
using UnityEngine;

public class Yellow_Boss_Projectile : MonoBehaviour
{
 
    public float damage;

    protected bool hasHit = false;
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null && !hasHit)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
                hasHit = true;
            }
        }
    }
}
=======
using UnityEngine;

public class Yellow_Boss_Projectile : MonoBehaviour
{
 
    public float damage;

    protected bool hasHit = false;
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null && !hasHit)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
                hasHit = true;
            }
        }
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
