public class WaterCommand : BaseCommand{
	
	public WaterCommand(int id, CommandTarget target, ICommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
		Target = target;
	}

	public override void Update() {
		base.Update();
	}

	public override void Perform() {
		if (Target.EnoughWater) Cancel();
		Worker.Water(Target);
	}
}