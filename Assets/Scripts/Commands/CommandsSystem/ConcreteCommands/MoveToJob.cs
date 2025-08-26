using UnityEngine;

public class MoveToJob : BaseCommand {
    private Vector2 _targetPos;

    private bool _isMoving;

    public MoveToJob(int id, Vector2 targetPos, ICommandService commandService, IUpdateService updateService, Worker worker = null) : base(id,
        commandService, updateService, worker) {
        _targetPos = targetPos;
        Type = CommandType.Move;
    }

    public override void Update() {
        inProgress |= Worker != null;
        if (!inProgress) {
            return;
        }

        Perform();
    }

    public override void Perform() {
        if (!Worker.HasPath(_targetPos)) {
            Worker.CurrentCommandId = -1;
            return;
        }

        if (!_isMoving) {
            Worker.MoveTo(_targetPos);
        }

        if (Worker.IsAtPosition(_targetPos)) {
            Cancel();
        }
    }
}