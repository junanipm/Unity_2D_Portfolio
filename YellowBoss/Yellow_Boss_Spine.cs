using System.Collections;
using UnityEngine;

public class Yellow_Boss_Spine : Yellow_Boss_Projectile
{
    private Transform targetPlayer;
    public float rotationSpeed = 20f;
    public float launchSpeed = 360f;
    public float initialDelay = 0.5f;
    public float lifeTime = 5f;
    private Vector3 launchDirection;
    
    private bool isLaunched = false;
 
    private bool isAligning = false;

    private const float SpriteRotationOffset = -90f; 

    public void SetTarget(Transform target)
    {
        targetPlayer = target;

        
    }

    public void StartAlignment()
    {
        isAligning = true;
    }
    public void Launch()
    {
        if (targetPlayer != null)
        {
            launchDirection = (targetPlayer.position - transform.position).normalized;

            float angle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg + SpriteRotationOffset;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        isLaunched = true;
        Destroy(gameObject, lifeTime);
    }

    

    void Update()
    {
        if (targetPlayer == null)
        {
            Destroy(gameObject);
            return;
        }

 
        if (isAligning && !isLaunched)
        {
 
            Vector3 direction = targetPlayer.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + SpriteRotationOffset; 
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
 
        if (isLaunched)
        {
 
            transform.position += launchDirection * launchSpeed * Time.deltaTime;
        }
    }
}
