using System;
using CodeBase.Services;
using UnityEngine;

public class Worker : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	
	private ICommand _currentCommand;
	
	private IMovable _mover;
	private IDestroyer _destroyer;
	private ISearcher _searcher;
	
	private IWorkerService _workerService;

	public ICommand CurrentCommand => _currentCommand;
	
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
		_workerService = ServiceLocator.Container.Single<IWorkerService>();
		_workerService.RegisterWorker(this);
	}
	
	private void OnDestroy() {
		var workerService = ServiceLocator.Container.Single<IWorkerService>();
		if (workerService != null) {
			workerService.UnregisterWorker(this);
		}
	}
	
	public void AddCapability(CommandType capability) {
		CommandCapabilities |= capability;
	}
	
	public bool CanPerformNow(CommandType commandType) {
		return ((CommandCapabilities & commandType) == commandType) && _currentCommand == null;
	}

	public void TakeCommand(ICommand command) {
		_currentCommand = command;
	}
	public void ReleaseCommand() {
		if (_currentCommand != null) {
			_mover.Stop();
			_currentCommand.RemoveWorker(this);
			_currentCommand = null;
		}
	}
	
	//Mover
	public bool TryMoveTo(Vector2 position) => _mover?.TryMoveTo(position) ?? false;
	public bool IsAtPosition(Vector2 position) => _mover?.IsAtPosition(position) ?? false;
	
	//Destroyer
	public void Hit(CommandTarget target) => _destroyer?.Hit(target);

	//Searcher
	public void Search(CommandTarget target) => _searcher?.Search(target);

	public IMovable Mover => _mover;
	public IDestroyer Destroyer => _destroyer;
	public ISearcher Searcher => _searcher;
}