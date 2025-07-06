public class SearchCommand : BaseCommand{
	
	public SearchCommand(int id, CommandService commandService, IUpdateService updateService, Worker worker = null) : base(id, commandService, updateService, worker) {
	}

	public override void Update() {
		base.Update();

		if (!Worker.HasPath(Target.transform.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}
		
		Worker.MoveTo(Target.transform.position);
		
		if (Worker.IsAtPosition(Target.transform.position)) {
			Worker.Search(Target);
		}
		
		if (Target.Searched) Cancel();
	}

	public override void Perform() {
		Worker.Search(Target);
		if (Target.Searched) Cancel();
	}

}