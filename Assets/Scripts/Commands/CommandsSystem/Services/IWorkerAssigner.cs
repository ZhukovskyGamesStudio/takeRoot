using System.Collections.Generic;

public interface IWorkerAssigner {
	HashSet<Worker> Workers { get; }
	public void RegisterWorker(Worker worker);
	public void UnregisterWorker(Worker worker);
}