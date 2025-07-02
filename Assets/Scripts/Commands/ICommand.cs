using System;
using System.Collections.Generic;

public interface ICommand {
	int Id { get; }
	CommandState State { get; }
	List<Worker> Workers { get; }
	void Execute();
	void TryResolve();
	void Complete();
	void Cancel();
	void RemoveWorker(Worker worker);
	void SetWorker(Worker worker);
}

public enum CommandState {
	InProgress,
	Failed,
	Completed,
	NeedResolve
}

[Flags][Serializable]
public enum CommandType
{
	None = 0,
	Debug = 1 << 0,
	Cancel = 1 << 1,
	Move = 1 << 2,
	Destroy = 1 << 3,
	Search = 1 << 4,
	MoveSelected = 1 << 5,
}
