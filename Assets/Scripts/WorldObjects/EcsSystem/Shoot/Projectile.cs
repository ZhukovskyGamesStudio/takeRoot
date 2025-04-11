using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Reached = Animator.StringToHash("TargetReached");

    [SerializeField]protected float speed;
    [SerializeField]protected int damage;
    
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
            animator.SetBool(Move, true);
            MakeMove();
        }
        else
        {
            animator.SetBool(Reached, true);
            animator.SetBool(Move, false);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("TargetReached") ||
                !(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)) return;

            animator.SetBool(Reached, false);
            OnTargetReached();
        }
    }

    protected abstract bool TargetReached();
    protected abstract void OnTargetReached();
    protected abstract void MakeMove();
    
}