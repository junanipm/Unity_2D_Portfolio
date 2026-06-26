using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
 
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EffectConverter(int playerMode)
    {
        animator.SetInteger("PlayerMode",playerMode);
        animator.SetTrigger("EnemyHit");
    }
}
