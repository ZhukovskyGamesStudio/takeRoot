using System;
using Unity.Netcode;
using UnityEngine;

public class WorkerAnimator : NetworkBehaviour, IAnimationStateReader {
    [SerializeField]
    private Animator _animator;
    
    [SerializeField]
    private ChangeMoodAnimator _changeMoodAnimator;

    private readonly int _hitSpeed = Animator.StringToHash("HitSpeed");

    private readonly int _idleStateHash = Animator.StringToHash("Idle");
    private readonly int _craftStateHash = Animator.StringToHash("Craft");
    private readonly int _waterStateHash = Animator.StringToHash("Water");
    private readonly int _searchStateHash = Animator.StringToHash("Search");
    private readonly int _hitStateHash = Animator.StringToHash("Hit");
    private readonly int _moveStateHash = Animator.StringToHash("Move");
    private readonly int _jumpStateHash = Animator.StringToHash("Jump");
    private readonly int _sleepStateHash = Animator.StringToHash("Sleep");

    public AnimatorState State { get; private set; }
    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public bool OnContactPointWhileMove;

    public void SetMood(Mood mood) => _changeMoodAnimator.SetMood(mood);
    
    
    [ClientRpc]
    private void SetTriggerClientRpc(int triggerHash) {
        _animator.SetTrigger(triggerHash);
    }
    public void PlayCraft() {
        SetTriggerClientRpc(_craftStateHash);
    }
    public void PlayBuild() {
        SetTriggerClientRpc(_craftStateHash); //TODO: make build animation
    }

    public void PlayWater() {
        SetTriggerClientRpc(_waterStateHash);
    }

    public void PlaySearch() {
        SetTriggerClientRpc(_searchStateHash);
    }

    public void PlayHit() {
        SetTriggerClientRpc(_hitStateHash);
    }

    public void PlayMove() {
        SetTriggerClientRpc(_moveStateHash);
    }

    public void PlaySleep() {
        SetTriggerClientRpc(_sleepStateHash);
    }

    public void ResetToIdle() {
        SetTriggerClientRpc(_idleStateHash);
        OnContactPointWhileMove = false;
    }

    public void DoJump() {
        SetTriggerClientRpc(_jumpStateHash);
    }

    public void SetHitSpeedMultiplier(float multiplier) {
        _animator.SetFloat(_hitSpeed, multiplier);
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
        } else if (stateHash == _waterStateHash) {
            state = AnimatorState.Water;
        } else if (stateHash == _craftStateHash) {
            state = AnimatorState.Craft;
        } else if (stateHash == _searchStateHash) {
            state = AnimatorState.Search;
        } else if (stateHash == _hitStateHash) {
            state = AnimatorState.Hit;
        } else if (stateHash == _jumpStateHash) {
            state = AnimatorState.Jump;
        } else if (stateHash == _sleepStateHash) {
            state = AnimatorState.Sleep;
        } else {
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
    Jump,
    Sleep,
    Attack
}