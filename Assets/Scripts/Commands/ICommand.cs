public interface ICommand {
	int Id { get; }
	CommandState State { get; }
	void Execute();
	void TryResolve();
	void Complete();
	void Cancel();
	void RemoveWorker(Worker worker);
}

public enum CommandState {
	InProgress,
	Failed,
	Completed,
	NeedResolve
}
