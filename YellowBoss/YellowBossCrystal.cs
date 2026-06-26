using System.Collections;
using UnityEngine;

public class YellowBossCrystal : Enemy
{
 
    public YellowBossCombat yellowBossCombat;
    bool break0;
    bool break1;
    bool break2;
    
    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    protected override void Start()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        yellowBossCombat = GameObject.Find("YellowBoss").GetComponent<YellowBossCombat>();
        break0 = false;
        break1 = false;
        break2 = false;
    }
    protected override void FixedUpdate()
    {
        if (enemyDead) return;

    }
    protected override void Update()
    {
        if (enemyDead) return;

        if (enemyLife <= 100 && !break0)
        {
            animator.SetTrigger("Break00");
            break0 = true;
        }
        else if (enemyLife <= 50 && !break1)
        {
            animator.SetTrigger("Break01");
            break1 = true;
        }
        else if(enemyLife <=0 && !break2)
        {
            animator.SetTrigger("Break02");
            break2 = true;
        }
    }
    private Coroutine damageEffectCoroutine;
    public override void EnemyOnDamaged(float damage, float distance)
    {
 
        if (damageEffectCoroutine != null)
            StopCoroutine(damageEffectCoroutine); 

        damageEffectCoroutine = StartCoroutine(DamageEffect(damage, 0));
 

        if (enemyLife <= 0)
        {
            EnemyDieTrigger();
            
        }
        if (enemyLife <= 100)
        {
            animator.SetTrigger("Break00");
        }
        else if (enemyLife <= 50)
        {
            animator.SetTrigger("Break01");
        }
        
    }
    protected override IEnumerator DamageEffect(float dmg, float distance)
    {


 
        enemyLife -= dmg;
        enemyDamaged = true;





 
        yield return new WaitForSeconds(0.5f);


        enemyDamaged = false;
    }
    protected override void EnemyDieTrigger()
    {
        if (enemyLife <= 0 && !enemyDead)
        {
            StartCoroutine(EnemyDie());
            enemyDead = true;
            SoundManager.instance.SFXPlay("crystal", audioClip);
        }
    }
    protected override IEnumerator EnemyDie()
    {
 
        animator.SetTrigger("Break02");
        gameObject.layer = 11;
        yield return new WaitForSeconds(1f);
        yellowBossCombat.crystalLife -= 1;
        Destroy(gameObject);

    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if( other.CompareTag("Player"))
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if(player != null)
            {
 
            }
 
        }
    }
}
