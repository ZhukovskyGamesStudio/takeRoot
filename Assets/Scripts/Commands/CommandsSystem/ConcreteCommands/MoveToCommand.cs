using UnityEngine;

public class MoveToCommand : BaseCommand {

	private readonly Vector2 _targetPos;


	public MoveToCommand(Vector2 targetPos, CommandService commandService, IUpdateService updateService, Worker worker = null) : base(commandService, updateService, worker) {
		_targetPos = targetPos;
	}

	public override void Update() {
		HandleWorker();
		inProgress = Worker != null;
		if (!inProgress) return;
		
		if (!Worker.TryMoveTo(_targetPos)) {
			Worker.CurrentCommandId = -1;
			return;
		}

		if (Worker.IsAtPosition(_targetPos)) 
			Cancel();
	}
}