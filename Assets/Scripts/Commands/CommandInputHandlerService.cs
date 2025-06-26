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
		if (PendingCommand == CommandType.None) return;
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			_commandService.HandleCommandRequest(PendingCommand);
		}
	}
}