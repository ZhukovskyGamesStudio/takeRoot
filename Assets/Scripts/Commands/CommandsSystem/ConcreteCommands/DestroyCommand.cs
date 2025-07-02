public class DestroyCommand : BaseCommand {
	public DestroyCommand(CommandTarget target, CommandService commandService, IUpdateService updateService, Worker worker = null) : base(commandService, updateService, worker) {
		this.target = target;
	}

	public override void Update() {
		base.Update();
		
		if (!inProgress) return;

		if (!Worker.TryMoveTo(target.transform.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}

		if (Worker.IsAtPosition(target.transform.position)) {
			Worker.Hit(target);
		}
		
		if (target.IsDead) Cancel();
	}
}