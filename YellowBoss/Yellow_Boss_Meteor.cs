using UnityEngine;

public class Yellow_Boss_Meteor : Yellow_Boss_Projectile
{
    private Vector3 initialDirection;
    public float projectileSpeed = 10f;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    GameObject meteorFire;
    public int meteorType = 0;
    public AudioClip audioClip;
    
    public void SetTarget(Vector3 target)
    {
 
 
        initialDirection = (target - transform.position).normalized;
    }
    void Update()
    {
 
 
        transform.position += initialDirection * projectileSpeed * Time.deltaTime;

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
                GameObject m_explosion = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(m_explosion, 1f);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
  
            if (meteorType == 0)
            {
                SoundManager.instance.SFXPlay("Explode", audioClip);
                GameObject m_explosion = Instantiate(explosion, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                Destroy(m_explosion, 1f);
                Destroy(gameObject);
            }
            else
            {
                SoundManager.instance.SFXPlay("Explode", audioClip);
                GameObject m_explosion = Instantiate(explosion, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                GameObject m_Fire = Instantiate(meteorFire, new Vector3(gameObject.transform.position.x, -4.7f, 0), Quaternion.identity);
                Destroy(m_explosion, 1f);
                Destroy(m_Fire, 3f);
                Destroy(gameObject);

            }
            
            
        } 
    }
}
