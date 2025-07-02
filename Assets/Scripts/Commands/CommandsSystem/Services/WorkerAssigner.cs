using System.Collections.Generic;
using System.Linq;

public class WorkerAssigner : IUpdatable, IWorkerAssigner {
	public HashSet<Worker> Workers { get; private set; } = new HashSet<Worker>();

	private readonly IUpdateService _update;
	private readonly CommandService _commands;
	private readonly ISelectionService _selection;

	public WorkerAssigner(IUpdateService update,CommandService commands) {
		_update = update;
		_commands = commands;
		_update.Register(this);
	}

	public void Update() {
		var commandsWithoutWorker = _commands.Commands.Where(c => c.Value.Worker == null);
		foreach (var kvp in commandsWithoutWorker) {
			var worker = FindAvailableWorker(kvp.Value.Type, kvp.Value.WithSelectedWorker);
			if (worker != null) {
				kvp.Value.AssignWorker(worker);
			}
		}
	}

	public Worker FindAvailableWorker(CommandType type, bool withSelectedWorker) {
		return withSelectedWorker ? 
			_selection.Selected.GetComponent<Worker>() : //TODO: cache worker
			Workers.FirstOrDefault(w => w.CanPerformNow(type));
	}

	public void RegisterWorker(Worker worker) => Workers.Add(worker);
	public void UnregisterWorker(Worker worker) => Workers.Remove(worker);
}