using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[DefaultExecutionOrder(1000)] //TODO: Remove
public class MoveToTarget : MonoBehaviour{
	public Mover mover;
	public Transform target;

	public bool CanMove;
	private void Update() {
		if(!CanMove) return;
		mover.MoveTo(target.transform.position);
	}
}