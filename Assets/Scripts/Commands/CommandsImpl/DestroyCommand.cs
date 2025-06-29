using UnityEngine;
using CodeBase.Services;

public class DestroyCommand : ICommand {
	public int Id { get; }
	public CommandState State { get; private set; } = CommandState.NeedResolve;
	
	private Worker _worker;
	private CommandTarget _target;
	private readonly IWorkerService _workerService;
	
	public DestroyCommand(int id, CommandTarget target, IWorkerService workerService) {
		Id = id;
		_target = target;
		_workerService = workerService;
	}
	
	public void Execute() {
		if (State is CommandState.Failed or CommandState.Completed or CommandState.NeedResolve) {
			return;
		}
		
		if (!_worker.IsAtPosition(_target.transform.position)) {
			_worker.TryMoveTo(_target.transform.position);
			return;
		}
		
		_worker.Hit(_target);
	}
	
	public void TryResolve() {
		if (_target == null) {
			State = CommandState.Failed;
			return;
		}
		
		if (_worker == null || !_worker.IsIdle) {
			_worker = _workerService.GetIdleWorkerWithCapability(CommandType.Destroy);
		}
		
		if (_worker == null) {
			return; // Попробуем в следующий раз
		}
		
		State = CommandState.InProgress;
	}
	
	public void Cancel() {
		State = CommandState.Failed;
		RemoveWorker(_worker);
	}
	
	public void RemoveWorker(Worker worker) {
		if (_worker == worker) {
			_worker = null;
			State = CommandState.NeedResolve;
		}
	}
}