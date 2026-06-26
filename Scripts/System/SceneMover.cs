<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMover : MonoBehaviour
{

    PlayerCombat playerCombat;
    void Awake()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }


    public void SceneMove(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
    
    
    
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMover : MonoBehaviour
{

    PlayerCombat playerCombat;
    void Awake()
    {
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }


    public void SceneMove(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
    
    
    
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
