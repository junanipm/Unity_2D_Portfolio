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
