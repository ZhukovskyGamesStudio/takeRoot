public class Search : BaseCommand{
	
	public Search(CommandService commandService, IUpdateService updateService, Worker worker = null) : base(commandService, updateService, worker) {
	}

	public override void Update() {
		base.Update();

		if (!Worker.TryMoveTo(target.transform.position)) {
			Worker.CurrentCommandId = -1;
			return;
		}
		
		if (Worker.IsAtPosition(target.transform.position)) {
			Worker.Search(target);
		}
		
		if (target.Searched) Cancel();
	}

}