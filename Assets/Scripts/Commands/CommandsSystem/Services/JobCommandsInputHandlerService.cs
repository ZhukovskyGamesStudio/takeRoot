using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Object = System.Object;

public class JobCommandsInputHandlerService : IJobCommandsInputHandlerService, IUpdatable {
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly IUpdateService _updateService;

	private int id = 0;
	public ReactiveProperty<JobType> PendingCommand { get; set; } = new ReactiveProperty<JobType>();
	public bool IsEnabled { get; set; } = true;

	public JobCommandsInputHandlerService(IInputService input, IPhysicsService physics, IUpdateService updateService) {
		_input = input;
		_physics = physics;
		_updateService = updateService;
		_updateService.Register(this);
	}
	public void Update() {
		if (!IsEnabled) return;
		if (Input.GetKeyDown(KeyCode.Escape)) {
			PendingCommand.Value = JobType.None;
		}
		if (PendingCommand.Value == JobType.None) return;
		
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			var target = _physics.Raycast<CommandTarget>(_input.GetWorldMousePosition(), Vector2.zero);
			if (target != null) {
				if (PendingCommand.Value == JobType.Cancel) {
					CancelCommand(target);
					return;
				}
				CreateCommand(target);
			}
		}
	}

	private void CancelCommand(CommandTarget target) {
		target.CancelJob();
	}

	private void CreateCommand(CommandTarget target) {
		target.TrySetJob(PendingCommand.Value);
	}
}