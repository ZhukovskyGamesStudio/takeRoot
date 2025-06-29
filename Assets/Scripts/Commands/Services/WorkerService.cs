using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkerService : IWorkerService {
	private readonly List<Worker> _workers = new List<Worker>();

	public void RegisterWorker(Worker worker) {
		if (worker != null && !_workers.Contains(worker)) {
			_workers.Add(worker);
			Debug.Log($"Registered worker: {worker.name}");
		}
	}
	public void UnregisterWorker(Worker worker) {
		if (_workers.Contains(worker)) {
			_workers.Remove(worker);
			Debug.Log($"Unregistered worker: {worker.name}");
		}
	}
	
	public List<Worker> GetAllWorkers() {
		return new List<Worker>(_workers);
	}

	public Worker GetIdleWorkerWithCapability(CommandType capability) {
		return _workers.FirstOrDefault(w => w.CanPerformNow(capability) && 
		                                    w.TryGetComponent(out Selectable selectable) && 
		                                    !selectable.Selected);
	}
} 