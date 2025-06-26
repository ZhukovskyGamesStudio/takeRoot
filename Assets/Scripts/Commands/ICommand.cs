public interface ICommand {
	public int Id {get;}
	public bool IsCompleted { get; }
	public void Execute();
	public bool IsAvailable();
}
