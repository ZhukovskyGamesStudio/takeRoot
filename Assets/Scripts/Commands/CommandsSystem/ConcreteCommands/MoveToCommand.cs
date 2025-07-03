using UnityEngine;

public class MoveToCommand : BaseCommand {

	private readonly Vector2 _targetPos;


	public MoveToCommand(int id, Vector2 targetPos, ICommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
		_targetPos = targetPos;
		Type = CommandType.Move;
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