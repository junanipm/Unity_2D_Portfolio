using UnityEngine;

public class YellowSkillStart : MonoBehaviour
{
    PlayerCombat playerCombat;
 
    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

 
    void YellowSkillLaunch()
    {
        playerCombat.YellowSkill();
    }
    void GameObjectDisable()
    {
        gameObject.SetActive(false);
    }
}
