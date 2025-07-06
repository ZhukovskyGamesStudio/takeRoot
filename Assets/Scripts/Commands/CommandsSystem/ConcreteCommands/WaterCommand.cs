public class WaterCommand : BaseCommand{
	
	public WaterCommand(int id, CommandTarget target, ICommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
		Target = target;
	}

	public override void Update() {
		base.Update();

		if (!inProgress) return;

		if (!Worker.HasPath(Target.InteractPosition.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}
		
		Worker.MoveTo(Target.InteractPosition.position);

		if (Worker.IsAtPosition(Target.InteractPosition.position)) {
			Worker.Water(Target);
		}
		
		if (Target.EnoughWater) Cancel();
	}
}