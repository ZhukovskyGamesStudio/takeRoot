using UnityEngine;

public class DestroyCommand : ICommand{
	
	private readonly IGameFactory _factory;
	private ICommand _moveCommand;
	public int Id { get; }
	public bool IsCompleted { get; private set; }
	public CommandPerformer Performer { get; private set; }
	public CommandTarget Target { get; private set; }
	

	public DestroyCommand(DestroyCommandParams commandParams, IGameFactory factory) {
		_factory = factory;
		Performer = commandParams.Performer;
		Target = commandParams.Target;
	}
	
	public void Execute() {
		if (_moveCommand == null) {
			var moveParams = new MoveCommandParams(Performer,
				new Vector2((int)Target.transform.position.x, (int)Target.transform.position.y));
			_moveCommand = _factory.CreateCommand(moveParams);
		}

		if (_moveCommand.IsCompleted) {
			Debug.Log($"Destroy {Target.name} at {Target.transform.position}");
			IsCompleted = true;
		}
		else _moveCommand.Execute();
	}

	public bool IsAvailable() {
		throw new System.NotImplementedException();
	}
}