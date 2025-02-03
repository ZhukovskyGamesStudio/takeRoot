using UnityEngine;

public class Animatable : ECSComponent {
    private static readonly int Explored = Animator.StringToHash("Explored");
    private static readonly int Damaged = Animator.StringToHash("Damaged");
    private static readonly int Died = Animator.StringToHash("Died");

    [field: SerializeField]
    public Animator Animator { get; private set; }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) { }

    public void TriggerExplored() {
        Animator.SetTrigger(Explored);
    }

    public void TriggerDamaged() {
        //Animator.SetTrigger(Damaged);
    }

    public void TriggerDied() {
        //Animator.SetTrigger(Died);
    }
}