<<<<<<< HEAD
using UnityEngine;

public class Enemy_Monster_Meteor : EnemyProjectile
{
    
    public override void Init(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        if (direction.x > 0)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        Destroy(gameObject, lifeTime);
    }

}
=======
using UnityEngine;

public class Enemy_Monster_Meteor : EnemyProjectile
{
    
    public override void Init(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction.normalized;

        if (direction.x > 0)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        Destroy(gameObject, lifeTime);
    }

}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
