using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CommandInputHandlerService : ICommandInputHandlerService, IUpdatable{
	private readonly IInputService _input;
	private readonly ICommandService _commandService;
	private readonly IPhysicsService _physics;

	public CommandType PendingCommand { get; set; }
	public bool IsEnabled { get; set; }

	public CommandInputHandlerService(IInputService input, IPhysicsService physics, ICommandService commandService, IUpdateService updateService) {
		_input = input;
		_commandService = commandService;
		_physics = physics;
		updateService.Register(this);
	}
	public void Update() {
		if (!IsEnabled) return;
		if (PendingCommand == CommandType.None) return;
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			ICommandTarget target = _physics.Raycast<ICommandTarget>(_input.GetWorldMousePosition(), Vector2.zero);
			if (target != null)
				_commandService.HandleCommandRequest(PendingCommand, _input.GetWorldMousePosition(), target);
		}
	}
}

[Flags]
public enum CommandType
{
	None = 0,
	Debug = 1 << 0,
	Move = 1 << 1,
	Destroy = 1 << 2,
}