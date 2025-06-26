using UnityEngine;

public class CommandParamsFactory : ICommandParamsFactory {
	private readonly IInputService _input;
	private readonly IDataProvider _data;
	private readonly ISelectionService _selection;
	private readonly IPhysicsService _physics;

	private string debugText = "Debug called";
	private float debugTime = 1f;

	public CommandParamsFactory(IInputService input, IDataProvider data, ISelectionService selection, IPhysicsService physics) {
		_input = input;
		_data = data;
		_selection = selection;
		_physics = physics;
	}

	public MoveCommandParams CreateMoveCommandParams() {
		if (_selection.Selected == null) return null;
		var selectedObj = _selection.Selected;
		if (!selectedObj.TryGetComponent(out CommandPerformer performer)) return null;

		MoveCommandParams moveParams = new MoveCommandParams(performer, _input.GetWorldMousePosition());
		return moveParams;
	}

	public DebugCommandParams CreateDebugCommandParams() {
		return new DebugCommandParams(debugText, debugTime, _input.GetWorldMousePosition());
	}

	public ICommandParams CreateDestroyCommandParams() {
		foreach (CommandPerformer performer in _data.CreaturesData.CommandPerformers) {
			if (!performer.CanPerform(CommandType.Destroy) || performer.IsPerforming) continue;

			var target = _physics.Raycast<CommandTarget>(_input.GetWorldMousePosition(), Vector2.zero);
			if (target == null) continue;
			
			DestroyCommandParams destroyParams = new DestroyCommandParams(performer, target);
			return destroyParams;
		}

		return null;
	}
}