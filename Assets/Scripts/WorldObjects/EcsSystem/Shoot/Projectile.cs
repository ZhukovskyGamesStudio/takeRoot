using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected static readonly int Move = Animator.StringToHash("Move");
    
    [SerializeField]protected float speed;
    [SerializeField]protected float damage;
    
    [SerializeField]protected Animator animator;
    
    protected Zombie target;

    public void Init(Zombie target)
    {
        this.target = target;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (!TargetReached())
        {
            animator.SetBool("Move", true);
            MakeMove();
        }
        else
        {
            animator.SetBool("TargetReached", true);
            animator.SetBool("Move", false);


            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("TargetReached") ||
                !(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)) return;

            animator.SetBool("TargetReached", false);
            OnTargetReached();
        }
    }

    protected abstract bool TargetReached();
    protected abstract void OnTargetReached();
    protected abstract void MakeMove();
    
}