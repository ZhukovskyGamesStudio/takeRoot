using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Serialization;

public class Worker : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	public WorkerAnimator WorkerAnimator { get; set; }

	public CommandType CommandType = CommandType.None;
	public int CurrentCommandId = -1; // when no command = -1
	public bool Performing;
	
	private IMovable _mover;
	private IDestroyer _destroyer;
	private ISearcher _searcher;
	private IWaterer _waterer;
	
	
	private void Start() {
		ServiceLocator.Container.Single<IWorkerAssigner>().RegisterWorker(this);
		WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
		if (TryGetComponent(out _mover)) {
			AddCapability(CommandType.Move);
		}
		if (TryGetComponent(out _destroyer)) {
			AddCapability(CommandType.Destroy);
			_destroyer.Init(WorkerAnimator);
		}
		if (TryGetComponent(out _searcher)) {
			AddCapability(CommandType.Search);
			_searcher.Init(WorkerAnimator);
		}
		if (TryGetComponent(out _waterer)) {
			AddCapability(CommandType.Water);
		}
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
	public void CancelCommand() {
		CurrentCommandId = -1;
		switch (CommandType) {
			case CommandType.Destroy:
				_destroyer.Cancel();
				break;
			case CommandType.Search:
				_searcher.Cancel();
				break;
		}
	}

	
	//Mover
	public void MoveTo(Vector2 position) {
		_mover.MoveTo(position, WorkerAnimator); //TODO: change animations play while moving
	}
	public bool HasPath(Vector2 position) => _mover.HasPath(position);
	public bool IsAtPosition(Vector2 position) => _mover.IsAtPosition(position);
	
	//Destroyer
	public void Hit(CommandTarget target) {
		_destroyer.Hit(target);
	}

	//Searcher
	public void Search(CommandTarget target) {
		_searcher.Search(target);
	}

	//Waterer
	public void Water(CommandTarget target) {
		ExecutedWithAnimation(AnimatorState.Water, _waterer.Water, target, () => WorkerAnimator.PlayWater(), 1f);
	}
	
	public IMovable Mover => _mover;
	public IDestroyer Destroyer => _destroyer;
	public ISearcher Searcher => _searcher;
	private void ExecutedWithAnimation<T>(AnimatorState targetState, Action<T> action, T parameter, Action playAnimation, float time) where T : CommandTarget {
		if (WorkerAnimator.State != AnimatorState.Idle || Performing) return;
		Performing = true;
		
		Action<AnimatorState> handler = null;
		handler = (state) => {
			if (state != targetState) return;
			action(parameter);
			Performing = false;
			parameter.SetPerform(false);
			WorkerAnimator.StateExited -= handler;
		};
		WorkerAnimator.StateExited += handler;
		parameter.SetPerform(true);
		StartCoroutine(PlayAnimationWithCallback(time, playAnimation)); //TODO: make time = performing time for each command type
	}

	private IEnumerator PlayAnimationWithCallback(float time, Action playAnimation) {
		var currentTime = time;
		playAnimation();
		while (currentTime > 0) {
			currentTime -= Time.deltaTime;
			yield return null;
		}
		WorkerAnimator.ResetToIdle();
	}
}