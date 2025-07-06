using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class DestroyCommand : BaseCommand {

	private int _savedTargetHealth;
	private Vector3 _workerStartLocation;
	
	public DestroyCommand(int id, CommandTarget target, CommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
		this.Target = target;
		_savedTargetHealth = (int)target.Health.currentHealth;
	}

	public override void Update() {
		base.Update();
		
		if (!inProgress) return;

		//TODO: move movement to base command
		if (!Worker.HasPath(Target.transform.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}
		Worker.MoveTo(Target.transform.position); 

		if (Worker.IsAtPosition(Target.transform.position)) {
			Worker.Hit(Target);
		}
		
		if (Target.IsDead) Cancel();
	}
}