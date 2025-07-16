using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Object = System.Object;

public class JobCommandsInputHandlerService : IJobCommandsInputHandlerService, IUpdatable {
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly ICommandService _commandService;
	private readonly IUpdateService _updateService;

	private int id = 0;
	public ReactiveProperty<CommandType> PendingCommand { get; set; } = new ReactiveProperty<CommandType>();
	public bool IsEnabled { get; set; } = true;

	public JobCommandsInputHandlerService(IInputService input, IPhysicsService physics, ICommandService commandService, IUpdateService updateService) {
		_input = input;
		_physics = physics;
		_commandService = commandService;
		_updateService = updateService;
		_updateService.Register(this);
	}
	public void Update() {
		if (!IsEnabled) return;
		if (Input.GetKeyDown(KeyCode.Escape)) {
			PendingCommand.Value = CommandType.None;
		}
		if (PendingCommand.Value == CommandType.None) return;
		
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			_commandService.HandleCommandRequest(PendingCommand.Value, false);
			var target = _physics.Raycast<CommandTarget>(_input.GetWorldMousePosition(), Vector2.zero);
			if (target != null)
				CreateCommand(target);
		}
	}

	private void CreateCommand(CommandTarget target) {
		switch (PendingCommand.Value) {
			case CommandType.Search:
				if (target.CanPerform(PendingCommand.Value))
					new SearchCommand(id++, target, _commandService, _updateService);
				break;
			case CommandType.Destroy:
				if (target.CanPerform(PendingCommand.Value))
					new DestroyCommand(id++, target, _commandService, _updateService);
				break;
			case CommandType.Move:
				if (target.CanPerform(PendingCommand.Value))
					new MoveToJob(id++, _input.GetWorldMousePosition(), _commandService, _updateService);
				break;
			case CommandType.Cancel:
				target.CurrentCommandId = -1;
				break;
		}
	}
}