<<<<<<< HEAD
using System.Collections;
using UnityEngine;

public class FindRange : MonoBehaviour
{
    [SerializeField] private float _findRadius = 5f;
    int _layerMask;

    private Enemy enemy;

    
    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        var layerIndex = LayerMask.NameToLayer("Player");
        _layerMask = 1 << layerIndex;

        StartCoroutine(Co_FindTargetInRange());
    }
    void Update()
    {
       
    }

    private IEnumerator Co_FindTargetInRange()
    {
        var waitForSeconds = new WaitForSeconds(Random.Range(0.05f, 0.08f));
        
        while(true)
        {
            var circleCastResult = Physics2D.CircleCast(transform.position, _findRadius, Vector2.zero, 0, _layerMask);

            if(circleCastResult.collider != null) 
            {
                enemy?.OnFindTargetInRange(circleCastResult.collider.gameObject);
            }
            else
            {
                enemy?.OnTargetLost();
            }
            yield return waitForSeconds;
        }
    }
}
=======
using System.Collections;
using UnityEngine;

public class FindRange : MonoBehaviour
{
    [SerializeField] private float _findRadius = 5f;
    int _layerMask;

    private Enemy enemy;

    
    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        var layerIndex = LayerMask.NameToLayer("Player");
        _layerMask = 1 << layerIndex;

        StartCoroutine(Co_FindTargetInRange());
    }
    void Update()
    {
       
    }

    private IEnumerator Co_FindTargetInRange()
    {
        var waitForSeconds = new WaitForSeconds(Random.Range(0.05f, 0.08f));
        
        while(true)
        {
            var circleCastResult = Physics2D.CircleCast(transform.position, _findRadius, Vector2.zero, 0, _layerMask);

            if(circleCastResult.collider != null) 
            {
                enemy?.OnFindTargetInRange(circleCastResult.collider.gameObject);
            }
            else
            {
                enemy?.OnTargetLost();
            }
            yield return waitForSeconds;
        }
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
