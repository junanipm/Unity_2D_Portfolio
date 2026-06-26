/*
using System.Collections;
using UnityEngine;


  
{
    /*
 
    private int comboCount;
    private bool isBuffered = false;
    public void OnAttackInput(PlayerCombat ctx)
    {
        if(ctx.isKnockedBack) return;
        if(!isBuffered)
        {
            ctx.anim.SetTrigger("Attack");
        }
    }
    public void OnAttackHit(PlayerCombat ctx)
    {
        foreach (var e in ctx.rangeCheck.enemiesInRange)
            e.EnemyOnDamaged(damage);

        comboCount++;
        if (comboCount < 3) {
            ctx.StartCoroutine(BufferNext());
        } else {
            comboCount = 0;
        }
    }
    private IEnumerator BufferNext() {
        isBuffered = true;
        yield return new WaitForSeconds(0.6f);
        isBuffered = false;
    }
    
}
*/