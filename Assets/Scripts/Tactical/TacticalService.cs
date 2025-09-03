using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TacticalService : ITacticalService, IUpdatable {
	private readonly IGridService _gridService;
	private readonly ISelectionService _selection;
	private readonly IUpdateService _update;
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;

	public TacticalService(IGridService gridService, ISelectionService selection, IUpdateService update, IInputService input, IPhysicsService physics) {
		_gridService = gridService;
		_selection = selection;
		_update = update;
		_input = input;
		_physics = physics;
		_update.Register(this);
	}

	public void SetTacticalForSelectedSettlers() {
		var selectable = _selection.SelectedReactive.Value;
		if (selectable == null) return;
		if (selectable is SettlerSelectable) {
			var data = (AI.SettlerData)selectable.GetData();
			data.tactical.IsTactical = !data.tactical.IsTactical;
		}
	}

	private void AddTacticalMovePosToSelectedSettlers(Vector3 pos) {
		var selectable = _selection.SelectedReactive.Value;
		if (selectable == null) return;
		if (_gridService.IsOccupiedPos(pos)) return;
		if (selectable is SettlerSelectable) {
			var data = (AI.SettlerData)selectable.GetData();
			data.tactical.Target = null;
			data.tactical.TacticalMovePos = pos;
			data.tactical.HasTacticalMovePos = true;
		}
	}

	private void AddTacticalAttackTargetToSelectedSettlers(AI.Zombie zombie) {
		var selectable = _selection.SelectedReactive.Value;
		if (selectable == null) return;
		if (selectable is SettlerSelectable) {
			var data = (AI.SettlerData)selectable.GetData();
			data.tactical.Target = zombie;
		}
	}

	public void Update() {
		if (_input.GetMouseButtonDown(MouseButton.Right)) {
			var zombie = _physics.Raycast<AI.Zombie>(_input.GetWorldMousePosition(), Vector2.zero);
			if (zombie == null)
				AddTacticalMovePosToSelectedSettlers(_input.GetWorldMousePosition());
			else
				AddTacticalAttackTargetToSelectedSettlers(zombie);
				
		}
	}

	public void Dispose() {
		_update.Unregister(this);
	}
}