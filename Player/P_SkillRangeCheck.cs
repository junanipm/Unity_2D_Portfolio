using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class P_SkillRangeCheck : RangeCheck
{
 

    protected PlayerCombat playerCombat;

    public List<Enemy> enemiesInSkillRange = new List<Enemy>();
    public List<FinalBossCombat> finalBossInSkillRange = new List<FinalBossCombat>();
    protected override void Start()
    {
        base.Start();
        playerCombat = GetComponentInParent<PlayerCombat>();
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
 
        if (enemy != null)
        {
            enemiesInSkillRange.Add(enemy);
            Debug.Log("적이 스킬 사정거리 내에 있음");
        }

        FinalBossCombat fBoss = other.GetComponent<FinalBossCombat>();
        if (fBoss != null)
        {
            finalBossInSkillRange.Add(fBoss);

        }


    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemiesInSkillRange.Remove(enemy);
 
        }

        FinalBossCombat fBoss = other.GetComponent<FinalBossCombat>();
        if (fBoss != null)
        {
            finalBossInSkillRange.Remove(fBoss);
            
        }
  

    }
    public bool EnemyInSkillRange => enemiesInSkillRange.Count > 0;

    protected override void Update()
    {
 
    }
    public void processCheck(int value)
    {
        if (animConverter.currentState == PlayerState.Purple)
        {
            if (value == 0)
            {
                boxCollider2D.enabled = false;
            }
            else if (value == 1)
            {   
                boxCollider2D.enabled = true;
                boxCollider2D.size = new Vector2(8.3f, 2.45f);
                boxCollider2D.offset = new Vector2(2.14f, 0.85f);
            }
            else if (value == 2)
            {
                boxCollider2D.enabled = true;
                boxCollider2D.size = new Vector2(8.6f, 3);
                boxCollider2D.offset = new Vector2(2.3f, 0.44f);
            }
        }
    }
}
