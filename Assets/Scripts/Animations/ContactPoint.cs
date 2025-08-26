using UnityEngine;

public class ContactPoint : MonoBehaviour {
    [SerializeField]
    private WorkerAnimator animator;

    public void SetContactPoint(int isContact) {
        animator.OnContactPointWhileMove = isContact == 1 ? true : false;
    }
}