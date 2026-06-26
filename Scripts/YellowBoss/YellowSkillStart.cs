<<<<<<< HEAD
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
=======
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
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
