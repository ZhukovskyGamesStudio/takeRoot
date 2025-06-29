using System;
using CodeBase.Services;
using UnityEngine;

public class Worker : MonoBehaviour {
	public bool IsIdle { get; private set; } = true;
	public string WorkerName => gameObject.name;
	public CommandType CommandCapabilities { get; private set; }
	
	private IMovable _mover;
	private IDestroyer _destroyer;
	private ICommand _currentCommand;
	
	public ICommand CurrentCommand => _currentCommand;
	
	private void Start() {
		_mover = GetComponent<IMovable>();
		_destroyer = GetComponent<IDestroyer>();
		
		if (_mover != null) {
			AddCapability(CommandType.Move);
		}
		if (_destroyer != null) {
			AddCapability(CommandType.Destroy);
		}
		
		var workerService = ServiceLocator.Container.Single<IWorkerService>();
		if (workerService != null) {
			workerService.RegisterWorker(this);
		}
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
	
	public void RemoveCapability(CommandType capability) {
		CommandCapabilities &= ~capability;
	}
	
	public bool CanPerform(CommandType commandType) {
		return (CommandCapabilities & commandType) == commandType;
	}
	
	public void RemoveCommand() {
		if (_currentCommand != null) {
			_currentCommand.RemoveWorker(this);
			_currentCommand = null;
			IsIdle = true;
		}
	}
	
	public bool TryMoveTo(Vector2 position) {
		return _mover?.TryMoveTo(position) ?? false;
	}
	
	public bool IsAtPosition(Vector2 position) {
		return _mover?.IsAtPosition(position) ?? false;
	}
	
	public void Hit(CommandTarget target) {
		_destroyer?.Hit(target);
	}
	
	public IMovable Mover => _mover;
	public IDestroyer Destroyer => _destroyer;
}