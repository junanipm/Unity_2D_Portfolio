using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
  
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine.Events;
using System.Linq;

public class PurpleBossCombat : YellowBossCombat
{
 
    PurpleBoss purpleBoss;
    [SerializeField]
    protected GameObject firePrecursor;
    [SerializeField]
    protected GameObject fire;
    [SerializeField]
    GameObject[] backGround;

    public GameObject powerCrystalPurple;
    bool bossCleared;
    protected override void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        purpleBoss = GetComponent<PurpleBoss>();
    }
    protected override void Start()
    {
        base.Start();
        
    }

 
    protected override void Update()
    {
        bossHPSlider.maxValue = bossMaxHP;
        bossHPSlider.value = bossCurrentHP;
    }
    public override void StartBossFight()
    {
        if (patternCoroutine == null)
        {
            Debug.Log("보스전 시작 준비: 4초 후 패턴 시작");
            StartCoroutine(StartBossFightWithDelay(1f));
            bossHPSlider.gameObject.SetActive(true);
            bossLocation = 0;
            boxCollider.enabled = true;
 
        }
    }
    protected override IEnumerator StartBossFightWithDelay(float delay)
    {
 
        yield return new WaitForSeconds(delay);

 
        isTeleportable = true;
        patternCoroutine = StartCoroutine(BossPatternMaster());
    }

    protected override void BossClear()
    {
                  GameObject[] enemyAttack = GameObject.FindGameObjectsWithTag("BossAttack");
        
        foreach (GameObject Attack in enemyAttack)
        {
 
            Destroy(Attack);
        }

        gameObject.layer = 11;
        boxCollider.enabled = false;
        StopAllCoroutines();
        bossCleared = true;
        StartCoroutine(ClearCoroutine(bossLocation));
    }
    protected IEnumerator ClearCoroutine(int location)
    {

        Vector3 startPosition = transform.position;
        Vector3 DieLocation;
        if (location == 0)
        {
            DieLocation = gameObject.transform.position + new Vector3(0, -3.5f, 0);
        }
        else
        {
            DieLocation = gameObject.transform.position + new Vector3(0, -5.5f, 0);
        }
        float duration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, DieLocation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = DieLocation;
        anim.SetTrigger("BossClear");
        yield return new WaitForSeconds(1f);
        gameObject.layer = 11;
        bossHPSlider.gameObject.SetActive(false);
        GameObject crystal = Instantiate(powerCrystalPurple, transform.position, transform.rotation);
        purpleBoss.PlayerPause(true);
    }
          protected override void UpdatePosition()
    {
 
        if (target != null)
        {
 
            playerPosition = target.position;
 
        }
    }
    protected override IEnumerator BossPatternMaster()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
 
            if (isTeleportable)
            {
                Debug.Log("텔포했어요");
                anim.ResetTrigger("Spine");
                anim.Play("Y_Boss_Idle");
                Teleport();
                GameObject bossTPEffect = Instantiate(bossTeleportEffect, gameObject.transform);
                Destroy(bossTPEffect, 1f);
                isTeleportable = false;

            }

            yield return new WaitForSeconds(0.5f);

            if (bossLocation == 0)
            {
 
                Spine();

 
                while (isAttacking)
                {
                    yield return null;
                }

 
                ForcedSideTeleport();
                anim.ResetTrigger("Spine");
                GameObject bossTPEffect = Instantiate(bossTeleportEffect, gameObject.transform);
                Destroy(bossTPEffect, 1f);
            }
            else 
            {
                float attackTimer = 0f;
                while (attackTimer < teleportCoolTime - 0.5f)
                {
                    if (!isAttacking)
                    {
                        PatternCalculation();
                    }

                    yield return new WaitForSeconds(attackCoolTime);
                    attackTimer += attackCoolTime;

                    if (attackTimer >= teleportCoolTime) break;
                }
            }

 
            isTeleportable = true;
        }
    }
    protected override void PatternCalculation()
    {
        if (bossLocation != 0) 
        {
            int rand = UnityEngine.Random.Range(0, 2); 

            if (rand == 0) 
            {
 
                StartCoroutine(ExecuteAttack(() => Meteor(bossLocation, playerPosition), 0.5f));
                
                anim.SetTrigger("Meteor");
            }
            else 
            {
 
                StartCoroutine(ExecuteAttack(() => Sphere(bossLocation), 0.5f));
                anim.SetTrigger("Sphere");
            }
        }
        else if (bossLocation == 0) 
        {
 
            if (!isAttacking)
            {
                Spine();
                anim.SetTrigger("Spine");
            }
        }
    }
    protected override IEnumerator ExecuteAttack(System.Action attackAction, float duration)
    {
 
        isAttacking = true; 
 
        anim.SetBool("isAttacking", true);
        attackAction.Invoke();

 
        yield return new WaitForSeconds(duration);

        isAttacking = false; 
        anim.SetBool("isAttacking", false);
    }
    protected override void ThunderPattern()
    {
        int patternValue = UnityEngine.Random.Range(0, 3);
        Debug.Log(patternValue);
        StartCoroutine(ThunderInstantiate(patternValue));
    }

    protected override IEnumerator ThunderInstantiate(int pattern)
    {
        if (pattern == 0)
        {
            GameObject precursor00 = Instantiate(PatternPrecursor[0]);
            yield return new WaitForSeconds(1f);
            Destroy(precursor00);
            GameObject ThunderPattern00 = Instantiate(bossThunder[0]);
            yield return new WaitForSeconds(1f);
            Destroy(ThunderPattern00);
        }
        else if (pattern == 1)
        {
            GameObject precursor01 = Instantiate(PatternPrecursor[1]);
            yield return new WaitForSeconds(1f);
            Destroy(precursor01);
            GameObject ThunderPattern01 = Instantiate(bossThunder[1]);
            yield return new WaitForSeconds(1f);
            Destroy(ThunderPattern01);
        }
        else if (pattern == 2)
        {
            GameObject precursor02 = Instantiate(PatternPrecursor[2]);
            yield return new WaitForSeconds(1f);
            Destroy(precursor02);
            GameObject ThunderPattern02 = Instantiate(bossThunder[2]);
            yield return new WaitForSeconds(1f);
            Destroy(ThunderPattern02);
        }
    }
    public override void ThunderStart()
    {
        StartCoroutine(RandomlyThunder());
        StartCoroutine(FireDelay());
    }
    protected override IEnumerator RandomlyThunder()
    {
        while (true)
        {
 
            float randomDelay = UnityEngine.Random.Range(thunderMinDelay, thunderMaxDelay);

            Debug.Log($"다음 실행까지 {randomDelay:F2}초 대기합니다.");

 
            yield return new WaitForSeconds(randomDelay);

 
            ThunderPattern();
        }
    }
    protected IEnumerator FireDelay()
    {
        while (true)

        {
            float randomDelay = UnityEngine.Random.Range(10, 12);

            yield return new WaitForSeconds(randomDelay);
            FirePattern();
        }
    }
    protected void FirePattern()
    {
        StartCoroutine(FireInstantiate(playerPosition));
    }
    protected IEnumerator FireInstantiate(Vector3 playerposition)
    {
        Vector3 targetPosition = playerPosition + new Vector3(0, 1F, 0);
        GameObject firePrecursorObject = Instantiate(firePrecursor, targetPosition -new Vector3(0,2.0f,0), gameObject.transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(firePrecursorObject);
        GameObject fireObject = Instantiate(fire, targetPosition, gameObject.transform.rotation);
        soundPlayer.SoundPlay(0);
        Destroy(fireObject, 1f);
    }
    
    protected IEnumerator MeteorSecond(int direction)
    {
        yield return new WaitForSeconds(1f);
        Vector3  playerPosition = target.position;
        if (direction == -1) 
        {
 
            meteorSpawnPosition = new Vector3(4f, 10f, 1f);
        }
        else if (direction == 1) 
        {
 
            meteorSpawnPosition = new Vector3(23f, 10f, 1f);
        }

        Vector3 directionVector = playerPosition - meteorSpawnPosition;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        GameObject newMeteorObject = Instantiate(bossMeteor, meteorSpawnPosition, targetRotation);
        soundPlayer.SoundPlay(3);
        Yellow_Boss_Meteor meteorScript = newMeteorObject.GetComponent<Yellow_Boss_Meteor>();
        if (meteorScript != null)
        {
            meteorScript.SetTarget(playerPosition);
        }
  
    }
    protected override void Meteor(int direction, Vector3 playerPosition)
    {

        if (direction == -1) 
        {
 
            meteorSpawnPosition = new Vector3(4f, 10f, 1f);
        }
        else if (direction == 1) 
        {
 
            meteorSpawnPosition = new Vector3(23f, 10f, 1f);
        }
        Vector3 directionVector = playerPosition - meteorSpawnPosition;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        GameObject newMeteorObject = Instantiate(bossMeteor, meteorSpawnPosition, targetRotation);
        soundPlayer.SoundPlay(3);
        Yellow_Boss_Meteor meteorScript = newMeteorObject.GetComponent<Yellow_Boss_Meteor>();
        if (meteorScript != null)
        {
            meteorScript.SetTarget(playerPosition);
        }
        StartCoroutine(MeteorSecond(direction));
    }

    protected override IEnumerator SpineCoroutine()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        anim.SetTrigger("Spine");
        activeSpines.Clear();
        

        for (int i = 0; i < spineAngles.Length; i++)
        {
            float targetAngle = spineAngles[i];

            Vector3 spawnPosition = spineSpawnPoint[i];

            GameObject spineObject = Instantiate(bossSpine, spawnPosition, Quaternion.Euler(0f, 0f, targetAngle));
            soundPlayer.SoundPlay(5);
            Yellow_Boss_Spine spineScript = spineObject.GetComponent<Yellow_Boss_Spine>();
            if (spineScript != null)
            {
                spineScript.SetTarget(target);
                activeSpines.Add(spineScript);
            }

            yield return new WaitForSeconds(0.3f);
        }
        foreach (var spine in activeSpines)
        {
            if (spine != null)
            {
                spine.StartAlignment();
            }
        }
        float spineDelay = UnityEngine.Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(spineDelay);

        for (int i = 0; i < activeSpines.Count; i++)
        {
            Yellow_Boss_Spine spine = activeSpines[i];

            if (spine != null)
            {
                soundPlayer.SoundPlay(6);
                spine.Launch();
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1.0f); 

 
        isAttacking = false; 
        anim.SetBool("isAttacking", false);
 
        Debug.Log("Spine Pattern Completed. isAttacking is now FALSE.");
        
 
        activeSpines.Clear(); 
    }

}
