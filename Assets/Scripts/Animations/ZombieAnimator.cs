using System;
using Unity.Netcode;
using UnityEngine;

public class ZombieAnimator : NetworkBehaviour, IAnimationStateReader{
	[SerializeField]private Animator _animator;
		
	private readonly int _idleStateHash = Animator.StringToHash("Idle");
	private readonly int _moveStateHash = Animator.StringToHash("Move");
	private readonly int _attackStateHash = Animator.StringToHash("Attack");
		
	public AnimatorState State { get; private set; }
		
	public event Action<AnimatorState> StateEntered;
	public event Action<AnimatorState> StateExited;
	public bool OnContactPointWhileMove;

	[ClientRpc]
	public void SetTriggerClientRpc(int triggerHash) {
		_animator.SetTrigger(triggerHash);
	}

	public void ResetToIdle() {
		SetTriggerClientRpc(_idleStateHash);
		OnContactPointWhileMove = false;
	}

	public void PlayMove() {
		SetTriggerClientRpc(_moveStateHash);
	}

	public void PlayAttack() {
		SetTriggerClientRpc(_attackStateHash);
	}


	public void EnteredState(int stateHash) {
		State = StateFor(stateHash);
		StateEntered?.Invoke(State);
	}

	public void ExitedState(int stateHash) {
		StateExited?.Invoke(StateFor(stateHash));
	}

	private AnimatorState StateFor(int stateHash) {
		AnimatorState state;
		if (stateHash == _moveStateHash) {
			state = AnimatorState.Move;
		} else if (stateHash == _idleStateHash) {
			state = AnimatorState.Idle;
		} else if (stateHash == _attackStateHash) {
			state = AnimatorState.Attack;
		} else {
			state = AnimatorState.None;
		}
		return state;
	} 
}