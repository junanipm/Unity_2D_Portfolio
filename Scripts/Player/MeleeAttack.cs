<<<<<<< HEAD
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
=======
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
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
*/