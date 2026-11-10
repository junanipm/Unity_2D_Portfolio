using UnityEngine;

public class FinalBoss_Projectile : MonoBehaviour
{
 
    public float damage;

    protected bool hasHit = false;
    public void Init(int direction)
    {
        if (direction < 0)
        {

            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {

            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
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
