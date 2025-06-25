using UnityEngine;

public class DebugCommand: ICommand {
	private readonly DebugCommandParams _params;

	public int Id { get;}
	
	public DebugCommand(DebugCommandParams commandParams, int id) {
		Id = id;
		_params = commandParams;
	}
	
	public void Execute() {
		throw new System.NotImplementedException();
	}

	public bool IsAvailable() {
		throw new System.NotImplementedException();
	}
}