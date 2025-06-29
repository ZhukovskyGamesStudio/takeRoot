using System.Collections.Generic;

public interface IWorkerService : IService {
	void RegisterWorker(Worker worker);
	void UnregisterWorker(Worker worker);
	Worker GetIdleWorkerWithCapability(CommandType capability);
	List<Worker> GetAllWorkers();
} 