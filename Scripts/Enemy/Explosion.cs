<<<<<<< HEAD
using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip audioClip;
 
    private float damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat playerCombat = collision.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
            }
        }

    }
    public void Init(float damage)
    {
        this.damage = damage;


    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    
    public virtual void EnemySoundPlay()
    {
        SoundManager.instance.SFXPlay("Attack", audioClip);
    }
}
=======
using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip audioClip;
 
    private float damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat playerCombat = collision.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(gameObject.transform.position, damage);
            }
        }

    }
    public void Init(float damage)
    {
        this.damage = damage;


    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    
    public virtual void EnemySoundPlay()
    {
        SoundManager.instance.SFXPlay("Attack", audioClip);
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
