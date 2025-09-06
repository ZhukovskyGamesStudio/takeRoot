using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TacticalService : ITacticalService, IUpdatable {
	private readonly IGridService _gridService;
	private readonly ISelectionService _selection;
	private readonly IUpdateService _update;
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly INetworkService _network;

	public TacticalService(IGridService gridService, ISelectionService selection, IUpdateService update, IInputService input, IPhysicsService physics, INetworkService network) {
		_gridService = gridService;
		_selection = selection;
		_update = update;
		_input = input;
		_physics = physics;
		_network = network;
		_update.Register(this);
	}

	public void SetTacticalForSelectedSettlers(bool isOn) {
		var selectable = _selection.SelectedReactive.Value;
		if (selectable == null) return;
		if (selectable is SettlerSelectable) {
			selectable.GetComponent<AI.Settler>().SetTacticalServerRpc(isOn);
		}
	}

	private void AddTacticalMovePosToSelectedSettlers(Vector3 pos) {
		var selectable = _selection.SelectedReactive.Value;
		if (selectable == null) return;
		if (_gridService.IsOccupiedPos(pos)) return;
		if (selectable is SettlerSelectable) {
			var data = selectable.GetData(_network.MyRace.Value);
			if (data is AI.SettlerData settlerData) {
				var posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z); 
				selectable.GetComponent<AI.Settler>().SetTacticalValuesServerRpc(posInt, true);
			}
		}
	}

	private void AddTacticalAttackTargetToSelectedSettlers(AI.Zombie zombie) {
		var selectable = _selection.SelectedReactive.Value;
		if (selectable == null) return;
		if (selectable is SettlerSelectable) {
			var data = (AI.SettlerData)selectable.GetData(_network.MyRace.Value);
			selectable.GetComponent<AI.Settler>().SetTacticalTargetServerRpc(zombie.Position);
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