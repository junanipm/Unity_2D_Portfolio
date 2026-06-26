using System.Collections;
using UnityEngine;

public class BlueBossLaser : MonoBehaviour
{
    private LineRenderer lr;

    public GameObject startPointParticle;
    public GameObject endPointParticle;
    
    public LayerMask collisionMask;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        if (endPointParticle != null && startPointParticle != null)
        {
            endPointParticle.SetActive(false);
            startPointParticle.SetActive(false);
        }
    }

    public void FireLaser(Vector3 startPoint, float targetX, float duration)
    {
        lr.enabled = true;
        lr.SetPosition(0, startPoint);

        if (endPointParticle != null && startPointParticle != null)
        {
            endPointParticle.SetActive(true);
            startPointParticle.SetActive(true);
        }

        Vector3 initialEndPoint = new Vector3(startPoint.x, -4.7f, startPoint.z);
        Vector3 targetEndPoint = new Vector3(targetX, -4.7f, startPoint.z);

        StartCoroutine(MoveEndPoint(initialEndPoint, targetEndPoint, duration));
    }

    private IEnumerator MoveEndPoint(Vector3 initialEndPoint, Vector3 targetEndPoint, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            Vector3 currentEndPoint = Vector3.Lerp(initialEndPoint, targetEndPoint, t);

 
            RaycastHit2D hit = Physics2D.Raycast(lr.GetPosition(0), currentEndPoint - lr.GetPosition(0), Vector3.Distance(lr.GetPosition(0), currentEndPoint), collisionMask);

            if (hit.collider != null)
            {
                PlayerCombat playerCombat = hit.collider.gameObject.GetComponent<PlayerCombat>();

 
                if (playerCombat != null)
                {
                    Debug.Log("Player를 감지했습니다. PlayerCombat 스크립트의 함수를 호출합니다.");
                    playerCombat.TakeDamage(gameObject.transform.position, 20);
                }
                
            }

 
            lr.SetPosition(1, currentEndPoint);
            if (endPointParticle != null)
            {
                endPointParticle.transform.position = currentEndPoint;
            }

            yield return null;
        }

 
        lr.SetPosition(1, targetEndPoint);
        if (endPointParticle != null)
        {
            endPointParticle.transform.position = targetEndPoint;
        }

        yield return new WaitForSeconds(0.2f);
        lr.enabled = false;
        if (endPointParticle != null && startPointParticle != null)
        {
            endPointParticle.SetActive(false);
            startPointParticle.SetActive(false);
        }
    }
}