using System;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Serialization;

public class Worker : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	public WorkerAnimator WorkerAnimator { get; private set; }
	
	public int CurrentCommandId = -1; // when no command = -1
	public bool Performing;
	
	private IMovable _mover;
	private IDestroyer _destroyer;
	private ISearcher _searcher;
	private IWaterer _waterer;
	
	
	private void Start() {
		if (TryGetComponent(out _mover)) {
			AddCapability(CommandType.Move);
		}
		if (TryGetComponent(out _destroyer)) {
			AddCapability(CommandType.Destroy);
		}
		if (TryGetComponent(out _searcher)) {
			AddCapability(CommandType.Search);
		}
		if (TryGetComponent(out _waterer)) {
			AddCapability(CommandType.Water);
		}
		//_workerService.RegisterWorker(this);
		WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
	}
	
	public void AddCapability(CommandType capability) {
		CommandCapabilities |= capability;
	}

	public void RemoveCapability(CommandType capability) {
		CommandCapabilities &= ~capability;
	}
	
	public bool CanPerformNow(CommandType commandType) {
		return ((CommandCapabilities & commandType) == commandType) && CurrentCommandId == -1;
	}

	
	//Mover
	public void MoveTo(Vector2 position) {
		if (WorkerAnimator.State == AnimatorState.Idle) {
			_mover.MoveTo(position);
		}
	}
	public bool HasPath(Vector2 position) => _mover.HasPath(position);
	public bool IsAtPosition(Vector2 position) => _mover.IsAtPosition(position);
	
	//Destroyer
	public void Hit(CommandTarget target) {
		ExecutedWithAnimation(AnimatorState.Hit, _destroyer.Hit, target, () => WorkerAnimator.PlayHit());
	}

	//Searcher
	public void Search(CommandTarget target) {
		ExecutedWithAnimation(AnimatorState.Search, _searcher.Search, target, () => WorkerAnimator.PlaySearch());
	}

	//Waterer
	public void Water(CommandTarget target) {
		ExecutedWithAnimation(AnimatorState.Water, _waterer.Water, target, () => WorkerAnimator.PlayWater());
	}
	
	public IMovable Mover => _mover;
	public IDestroyer Destroyer => _destroyer;
	public ISearcher Searcher => _searcher;

	private void ExecutedWithAnimation<T>(AnimatorState targetState, Action<T> action, T parameter, Action playAnimation) {
		if (WorkerAnimator.State != AnimatorState.Idle || Performing) return;
		Performing = true;
		
		Action<AnimatorState> handler = null;
		handler = (state) => {
			if (state != targetState) return;
			action(parameter);
			WorkerAnimator.StateExited -= handler;
			Performing = false;
			WorkerAnimator.ResetToIdle();
		};
		WorkerAnimator.StateExited += handler;
		playAnimation();
	}
}