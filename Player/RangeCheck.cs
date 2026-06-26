using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    protected BoxCollider2D boxCollider2D;
    protected AnimatorConverter animConverter;
    protected virtual void Start()
    {
        animConverter = GetComponentInParent<AnimatorConverter>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public List<Enemy> enemiesInRange = new List<Enemy>();
    public List<BlueBossCombat> bossesInRange = new List<BlueBossCombat>();
    public List<YellowBossCombat> yBossInRanage = new List<YellowBossCombat>();
    public List<FinalBossCombat> fBossInRanage = new List<FinalBossCombat>();
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
 
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
 
        }
        if (other.CompareTag("Boss"))
        {
            BlueBossCombat boss = other.GetComponent<BlueBossCombat>();
            if (boss != null)
            {
                bossesInRange.Add(boss);
 
            }
            YellowBossCombat yBoss = other.GetComponent<YellowBossCombat>();
            if (yBoss != null)
            {
                yBossInRanage.Add(yBoss);
 
            }
            FinalBossCombat fBoss = other.GetComponent<FinalBossCombat>();
            if (fBoss != null)
            {
                fBossInRanage.Add(fBoss);
            }
        }

    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        
        if(enemy != null)
        {
            enemiesInRange.Remove(enemy);
  
        }

        if (other.CompareTag("Boss"))
        {
            BlueBossCombat boss = other.GetComponent<BlueBossCombat>();
            if (boss != null)
            {
                bossesInRange.Remove(boss);
            }
            YellowBossCombat yBoss = other.GetComponent<YellowBossCombat>();
            if (yBoss != null)
            {
                yBossInRanage.Remove(yBoss);
 
            }
            FinalBossCombat fBoss = other.GetComponent<FinalBossCombat>();
            if (fBoss != null)
            {
                fBossInRanage.Remove(fBoss);
            }
        }
        
    }
    public bool EnemyInRange => enemiesInRange.Count > 0 || bossesInRange.Count > 0;

    protected virtual void Update()
    {
        colliderPerMode(animConverter.currentState);
    }
    void colliderPerMode(PlayerState playerState)
    {
        if((int)playerState == 1)
        {
            boxCollider2D.size = new Vector2(3.12f, 1.4f);
            boxCollider2D.offset = new Vector2(0.46f, 0.8f);
        }
        else if((int)playerState == 3)
        {
            boxCollider2D.size = new Vector2(4.7f, 1.4f);
            boxCollider2D.offset = new Vector2(2f, 0.5f);
        }
    }
}
