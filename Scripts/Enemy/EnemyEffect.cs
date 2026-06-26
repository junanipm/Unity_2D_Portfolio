<<<<<<< HEAD
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
=======
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
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
