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
