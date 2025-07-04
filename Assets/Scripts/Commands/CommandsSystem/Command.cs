using System;

public abstract class BaseCommand : IUpdatable {
	public int Id;
	public CommandType Type;
	
	public Worker Worker;
	protected CommandTarget Target;

	public readonly bool IsManualAssignment;

	protected bool inProgress;
	private readonly ICommandService _commandService;
	private readonly IUpdateService _updateService;

	public event Action onComplete;

	public BaseCommand(int id, ICommandService commandService, IUpdateService updateService, Worker worker = null) {
		Id = id;
		if (worker != null) {
			this.Worker = worker;
			worker.CurrentCommandId = Id;
			IsManualAssignment = true;
		}
		else IsManualAssignment = false;
		_commandService = commandService;
		_commandService.RegisterCommand(Id, this);
		_updateService = updateService;
		_updateService.Register(this);
	}
	
	public virtual void Update() {
		HandleWorker();
		HandleTarget();
		inProgress = Worker != null && Target != null;
	}

	private void HandleTarget() {
		if (Target != null && Target.CurrentCommandId == -1) {
			Target = null;
			Cancel();
		}
		if (Target == null) {
			Cancel();	
		}
	}

	protected void HandleWorker() {
		if (Worker != null && Worker.CurrentCommandId == -1) {
			Worker = null;
			if (IsManualAssignment) {
				Cancel();
			}
		}
	}

	public void AssignWorker(Worker worker) {
		this.Worker = worker;
		this.Worker.CurrentCommandId = Id;
	}

	public virtual void Cancel() {
		if (Worker != null) Worker.CurrentCommandId = -1;
		if (Target != null) Target.CurrentCommandId = -1;
		_updateService.Unregister(this);
		_commandService.UnregisterCommand(Id);
		
		onComplete?.Invoke();
	}

	public void Redo() => Update();
	//public abstract void Undo();
}

[Flags][Serializable]
public enum CommandType
{
	None = 0,
	Cancel = 1 << 1,
	Move = 1 << 2,
	Destroy = 1 << 3,
	Search = 1 << 4,
	Water = 1 << 5
}