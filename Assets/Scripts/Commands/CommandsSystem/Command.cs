public class BaseCommand : IUpdatable {
	public int Id;
	public CommandType Type;
	
	public Worker Worker;
	protected CommandTarget target;

	public bool WithSelectedWorker;

	protected bool inProgress;
	private readonly ICommandService _commandService;
	private readonly IUpdateService _updateService;

	public BaseCommand(ICommandService commandService, IUpdateService updateService, Worker worker = null) {
		if (worker != null) {
			this.Worker = worker;
			WithSelectedWorker = true;
		}
		else WithSelectedWorker = false;

		_commandService = commandService;
		_commandService.RegisterCommand(Id, this);
		_updateService = updateService;
		_updateService.Register(this);
	}
	
	public virtual void Update() {
		HandleWorker();
		HandleTarget();
		inProgress = Worker != null && target != null;
	}

	private void HandleTarget() {
		if (target != null && target.CurrentCommandId == -1) {
			target = null;
			Cancel();
		}
		if (target == null) {
			Cancel();	
		}
	}

	protected void HandleWorker() {
		if (Worker != null && Worker.CurrentCommandId == -1) {
			Worker = null;
			if (WithSelectedWorker) {
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
		if (target != null) target.CurrentCommandId = -1;
		_updateService.Unregister(this);
		_commandService.UnregisterCommand(Id);
	}
}