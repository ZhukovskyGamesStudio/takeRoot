using System;
using TMPro;
using UnityEngine;

public class WorkerAnimator : MonoBehaviour, IAnimationStateReader{
	
	[SerializeField]private Animator _animator;
	
	private readonly int _idleStateHash = Animator.StringToHash("Idle");
	private readonly int _craftStateHash = Animator.StringToHash("Craft");
	private readonly int _waterStateHash = Animator.StringToHash("Water");
	private readonly int _searchStateHash = Animator.StringToHash("Search");
	private readonly int _hitStateHash = Animator.StringToHash("Hit");
	private readonly int _moveStateHash = Animator.StringToHash("Move");
	private readonly int _jumpStateHash = Animator.StringToHash("Jump");
	
	public AnimatorState State { get; private set; }
	public event Action<AnimatorState> StateEntered;
	public event Action<AnimatorState> StateExited;

	public bool OnContactPointWhileMove;

	public void PlayCraft() => _animator.SetTrigger(_craftStateHash);
	public void PlayWater() => _animator.SetTrigger(_waterStateHash);
	public void PlaySearch() => _animator.SetTrigger(_searchStateHash);
	public void PlayHit() => _animator.SetTrigger(_hitStateHash);
	public void PlayMove() => _animator.SetTrigger(_moveStateHash);
	public void ResetToIdle() {
		_animator.SetTrigger(_idleStateHash);
		OnContactPointWhileMove = false;
	}

	public void DoJump() => _animator.SetTrigger(_jumpStateHash);


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
		}
		else if (stateHash == _idleStateHash) {
			state = AnimatorState.Idle;
		}		
		else if (stateHash == _waterStateHash) {
			state = AnimatorState.Water;
		}
		else if (stateHash == _craftStateHash) {
			state = AnimatorState.Craft;
		}
		else if (stateHash == _searchStateHash) {
			state = AnimatorState.Search;
		}
		else if (stateHash == _hitStateHash) {
			state = AnimatorState.Hit;
		}
		else if (stateHash == _jumpStateHash) {
			state = AnimatorState.Jump;
		}
		else {
			state = AnimatorState.None;
		}
		return state;
	}
}

[Serializable]
public enum AnimatorState {
	None,
	Idle,
	Move,
	Hit,
	Craft,
	Water,
	Search,
	Jump
}