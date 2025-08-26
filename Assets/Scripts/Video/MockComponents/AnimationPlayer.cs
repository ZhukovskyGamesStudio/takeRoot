using UnityEngine;

public class AnimationPlayer : MonoBehaviour {
    public AnimatorState State;
    public WorkerAnimator workerAnimator;

    private void Update() {
        if (State == AnimatorState.Craft) {
            workerAnimator.PlayCraft();
        } else if (State == AnimatorState.Water) {
            workerAnimator.PlayWater();
        }
    }
}