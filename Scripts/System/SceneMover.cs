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
