public class DestroyCommandParams : ICommandParams {
	
	public CommandPerformer Performer {get; set;}
	public CommandTarget Target { get; set; }

	public DestroyCommandParams(CommandPerformer performer, CommandTarget target) {
		Performer = performer;
		Target = target;
	}
}