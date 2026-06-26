<<<<<<< HEAD
using UnityEngine;

public class StrikeEffect : BossEffect
{
 
    public float speed;
    Vector2 direction = new Vector2 (-1,0);
 
    void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime);
    }
    override protected void Start()
    {
        Destroy(gameObject, 5f);
    }
}
=======
using UnityEngine;

public class StrikeEffect : BossEffect
{
 
    public float speed;
    Vector2 direction = new Vector2 (-1,0);
 
    void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime);
    }
    override protected void Start()
    {
        Destroy(gameObject, 5f);
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
