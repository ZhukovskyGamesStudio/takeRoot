public class Search : BaseCommand{
	
	public Search(int id, CommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
	}

	public override void Update() {
		base.Update();

		if (!Worker.TryMoveTo(Target.transform.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}
		
		if (Worker.IsAtPosition(Target.transform.position)) {
			Worker.Search(Target);
		}
		
		if (Target.Searched) Cancel();
	}

}