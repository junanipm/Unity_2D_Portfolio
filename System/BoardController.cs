using System.Collections;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    
    BoxCollider2D boxCol;
    float ignoreTime = 0.3f;
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        
    }
    public void BoxColEnable()
    {
 
        Debug.Log("아무일도 일어나지않았다");
    }
    public IEnumerator ColCorutine()
    {
        gameObject.layer = 16;
        
        yield return new WaitForSeconds(ignoreTime);
        gameObject.layer = 15;
        
    }
}

