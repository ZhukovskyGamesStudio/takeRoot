public class DestroyCommand : BaseCommand {
	public DestroyCommand(int id, CommandTarget target, CommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
		this.Target = target;
	}

	public override void Update() {
		base.Update();
		
		if (!inProgress) return;

		if (!Worker.TryMoveTo(Target.transform.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}

		if (Worker.IsAtPosition(Target.transform.position)) {
			Worker.Hit(Target);
		}
		
		if (Target.IsDead) Cancel();
	}
}