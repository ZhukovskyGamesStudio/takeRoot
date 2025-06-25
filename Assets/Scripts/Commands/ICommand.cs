public interface ICommand {
	public int Id {get;}
	public void Execute();
	public bool IsAvailable();
}
