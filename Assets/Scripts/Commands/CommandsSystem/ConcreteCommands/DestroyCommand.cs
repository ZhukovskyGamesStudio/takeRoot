using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class DestroyCommand : BaseCommand {

	private int _savedTargetHealth;
	private Vector3 _workerStartLocation;
	
	public DestroyCommand(int id, CommandTarget target, ICommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
		this.Target = target;
		Target.CurrentJobId = id;
		Type = CommandType.Destroy;
	}

	public override void Update() {
		base.Update();
	}

	public override void Perform() {
		Worker.Hit(Target);
		if (Target.IsDead) Cancel();
	}
}