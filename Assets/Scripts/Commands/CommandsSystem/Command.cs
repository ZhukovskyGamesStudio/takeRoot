using System;

[Obsolete]
public abstract class BaseCommand : IUpdatable {
    public int Id;
    public Worker Worker;
    public CommandType Type;

    protected CommandTarget Target;

    public readonly bool IsManualAssignment;

    protected bool inProgress;
    private readonly ICommandService _commandService;
    private readonly IUpdateService _updateService;

    public event Action onComplete;

    public BaseCommand(int id, ICommandService commandService, IUpdateService updateService, Worker worker = null) {
        Id = id;
        if (worker != null) {
            Worker = worker;
            worker.CurrentCommandId = Id;
            IsManualAssignment = true;
        } else {
            IsManualAssignment = false;
        }

        _commandService = commandService;
        _updateService = updateService;

        //_commandService.RegisterCommand(Id, this);
        _updateService.Register(this);
    }

    public virtual void Update() {
        HandleWorker();
        HandleTarget();
        inProgress = Worker != null && Target != null;

        if (!inProgress) {
            return;
        }

        Execute();
    }

    private void Execute() {
        //if (!Worker.HasPath(Target.InteractPosition.Value)) {
        //    Worker.CurrentCommandId = -1;
        //    return;
        //}
//
        //Worker.MoveTo(Target.InteractPosition.Value);
        //if (Worker.IsAtPosition(Target.InteractPosition.Value)) {
        //    Perform();
        //}
    }

    private void HandleTarget() {
        if (Target != null && Target.CurrentJobId == -1) {
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
        Worker = worker;
        Worker.CurrentCommandId = Id;
        Worker.CommandType = Type;
    }

    public virtual void Cancel() {
        if (Worker != null) {
            Worker.CancelCommand();
        }

        if (Target != null) {
            Target.CurrentJobId = -1;
        }

        _updateService.Unregister(this);
        //_commandService.UnregisterJob(Id);

        onComplete?.Invoke();
    }

    public abstract void Perform();

    public void Redo() {
        Update();
    }
    //public abstract void Undo();
    public void Dispose() {
        _updateService.Unregister(this);
    }
}

[Flags, Serializable]
public enum CommandType {
    None = 0,
    Cancel = 1 << 1,
    Move = 1 << 2,
    Destroy = 1 << 3,
    Search = 1 << 4,
    Water = 1 << 5,
    Carry = 1 << 6
}