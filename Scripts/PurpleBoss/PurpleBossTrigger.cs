<<<<<<< HEAD
using UnityEngine;

public class PurpleBossTrigger : YellowBossTrigger
{
 
    public PurpleBoss purpleBoss;
    protected override void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        purpleBoss = GameObject.Find("PurpleBoss").GetComponent<PurpleBoss>();
    }

 
    protected override void Update()
    {
        PlayerPositionFounder();
    }
    protected override void PlayerPositionFounder()
    {
        if (playerController.gameObject.transform.position.x >= 25f && !hasPaused)
        {
            hasPaused = true;

            purpleBoss.BossRevival();
            bossWall.SetActive(true);
            playerController.PlayerIdlePlay();
        }
    }
}
=======
using UnityEngine;

public class PurpleBossTrigger : YellowBossTrigger
{
 
    public PurpleBoss purpleBoss;
    protected override void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        purpleBoss = GameObject.Find("PurpleBoss").GetComponent<PurpleBoss>();
    }

 
    protected override void Update()
    {
        PlayerPositionFounder();
    }
    protected override void PlayerPositionFounder()
    {
        if (playerController.gameObject.transform.position.x >= 25f && !hasPaused)
        {
            hasPaused = true;

            purpleBoss.BossRevival();
            bossWall.SetActive(true);
            playerController.PlayerIdlePlay();
        }
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
