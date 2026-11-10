using UnityEngine;
using Unity.Cinemachine;

public class YellowBossTrigger : MonoBehaviour
{
    
    public YellowBoss yellowBoss;
 
    protected PlayerCombat playerCombat;
    protected PlayerController playerController;
    [SerializeField]
    public GameObject bossWall;
    protected bool hasPaused = false;


    protected virtual void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        yellowBoss = GameObject.Find("YellowBoss").GetComponent<YellowBoss>();
        

    }

 
    protected virtual void Update()
    {
        PlayerPositionFounder();
    }

    protected virtual void PlayerPositionFounder()
    {
        if (playerController.gameObject.transform.position.x >= 0.6f && !hasPaused)
        {
            hasPaused = true;
            
            yellowBoss.BossStart();
            bossWall.SetActive(true);
            playerController.PlayerIdlePlay();
        }
    }
    
}
