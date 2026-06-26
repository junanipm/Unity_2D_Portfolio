using UnityEngine;

public class BossEffect : MonoBehaviour
{
    public float lifeTime = 0.25f;

    public float damage;
    public int effectType = 0;
    protected bool hasHit = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if(playerCombat != null && !hasHit)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
                if(effectType == 0)hasHit = true;
            }
        }
    }
    virtual protected void Start()
    {
        hasHit = false;
        if (effectType == 0)
            Destroy(gameObject, lifeTime);
    }
    public void Init(float damage)
    {
        this.damage = damage;
        
    }

}
