using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class JobCommandsInputHandlerService : IJobCommandsInputHandlerService, IUpdatable {
	private readonly IInputService _input;
	private readonly ICommandService _commandService;
	private readonly IPhysicsService _physics;

	public ReactiveProperty<CommandType> PendingCommand { get; set; } = new ReactiveProperty<CommandType>();
	public bool IsEnabled { get; set; }

	public JobCommandsInputHandlerService(IInputService input, IPhysicsService physics, ICommandService commandService, IUpdateService updateService) {
		_input = input;
		_commandService = commandService;
		_physics = physics;
		updateService.Register(this);
	}
	public void Update() {
		if (!IsEnabled) return;
		if (Input.GetKeyDown(KeyCode.Escape)) {
			PendingCommand.Value = CommandType.None;
		}
		if (PendingCommand.Value == CommandType.None) return;
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			_commandService.HandleCommandRequest(PendingCommand.Value, false);
		}

	}
}