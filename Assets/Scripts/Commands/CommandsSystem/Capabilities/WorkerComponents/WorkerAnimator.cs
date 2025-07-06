using System;
using TMPro;
using UnityEngine;

public class WorkerAnimator : MonoBehaviour, IAnimationStateReader{
	
	[SerializeField]private Animator _animator;
	
	private readonly int _idleStateHash = Animator.StringToHash("Idle");
	private readonly int _craftStateHash = Animator.StringToHash("Craft");
	private readonly int _waterStateHash = Animator.StringToHash("Water");
	

	
	public AnimatorState State { get; private set; }

	public event Action<AnimatorState> StateEntered;
	public event Action<AnimatorState> StateExited;


	public void PlayCraft() {
		_animator.SetTrigger(_craftStateHash);
	}

	public void PlayWater() {
		_animator.SetTrigger(_waterStateHash);
	}
	
	public void ResetToIdle() {
		_animator.SetTrigger(_idleStateHash);
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
		if (stateHash == _idleStateHash) {
			state = AnimatorState.Idle;
		}
		else if (stateHash == _waterStateHash) {
			state = AnimatorState.Water;
		}
		else if (stateHash == _craftStateHash) {
			state = AnimatorState.Craft;
		}
		else {
			state = AnimatorState.None;
		}
		return state;
	}
}

public interface IAnimationStateReader {
	public AnimatorState State { get; }
	public void EnteredState(int stateHash);
	public void ExitedState(int stateHash);
}

[Serializable]
public enum AnimatorState {
	None,
	Idle,
	Move,
	Destroy,
	Craft,
	Water
}