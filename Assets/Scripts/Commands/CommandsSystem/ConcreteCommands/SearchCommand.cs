public class SearchCommand : BaseCommand {
    public SearchCommand(int id, CommandTarget target, ICommandService commandService, IUpdateService updateService, Worker worker = null) :
        base(id, commandService, updateService, worker) {
        Type = CommandType.Search;
        Target = target;
        Target.CurrentJobId = id;
    }

    public override void Update() {
        base.Update();
    }

    public override void Perform() {
        if (Target.Searched) {
            Cancel();
            return;
        }

        Worker.Search(Target);
    }
}