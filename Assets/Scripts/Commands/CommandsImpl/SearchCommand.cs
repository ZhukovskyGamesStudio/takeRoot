using UnityEngine;
using CodeBase.Services;

public class SearchCommand : ICommand {
	public int Id { get; }
	public CommandState State { get; private set; } = CommandState.NeedResolve;
	
	private Worker _worker;
	private CommandTarget _target;
	private readonly IWorkerService _workerService;
	
	public SearchCommand(int id, CommandTarget target, IWorkerService workerService) {
		Id = id;
		_target = target;
		_workerService = workerService;
	}
	
	public void Execute() {
		if (State is CommandState.Failed or CommandState.Completed or CommandState.NeedResolve) {
			return;
		}
		
		if (_target == null) {
			State = CommandState.Failed;
			return;
		}
		
		if (_worker == null) {
			State = CommandState.NeedResolve;
			return;
		}
		
		if (!_worker.IsAtPosition(_target.transform.position)) {
			_worker.TryMoveTo(_target.transform.position);
			return;
		}
		
		_worker.Search(_target);
		
		// Проверяем, завершен ли поиск
		var searchable = _target.GetComponent<ISearchableObj>();
		if (searchable != null && searchable.IsSearched) {
			State = CommandState.Completed;
		}
	}
	
	public void TryResolve() {
		if (_target == null) {
			State = CommandState.Failed;
			return;
		}
		
		if (_worker == null || !_worker.CanPerformNow(CommandType.Search)) {
			_worker = _workerService.GetIdleWorkerWithCapability(CommandType.Search);
			_worker?.TakeCommand(this);
		}
		
		if (_worker == null) {
			return;
		}
		
		State = CommandState.InProgress;
	}

	public void Complete() {
		throw new System.NotImplementedException();
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