using UnityEngine;

public class ChangeMoodAnimator : MonoBehaviour {
    private static readonly int mood = Animator.StringToHash("Mood");

    public Mood currentMood;
    public Animator animator;

    private void Start() {
        animator.enabled = false; //TODO: fix mood animation not working when trigger changed
        animator.enabled = true;
    }

    public void Update() {
        if (currentMood == Mood.Angry && animator.GetInteger(mood) != 2) {
            animator.SetInteger(mood, 2);
        } else if (currentMood == Mood.Happy && animator.GetInteger(mood) != 3) {
            animator.SetInteger(mood, 3);
        }
    }
}