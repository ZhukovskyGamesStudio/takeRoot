using UnityEngine;

public class ChangeMoodAnimator : MonoBehaviour{
	private static readonly int mood = Animator.StringToHash("Mood");

	public Mood currentMood;
	public Animator animator;
	
	public void Update() {
		if (currentMood == Mood.Angry) {
			animator.SetInteger(mood, 2);
		}
		else if (currentMood == Mood.Happy) {
			animator.SetInteger(mood, 3);
		}
	}
	
}