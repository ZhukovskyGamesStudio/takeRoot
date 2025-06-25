using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SelectionService : ISelectionService, IUpdatable
{
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	public ISelectableObj Selected { get; private set; }

	public SelectionService(IInputService inputService, IPhysicsService physics, IUpdateService updateService) {
		_input = inputService;
		_physics = physics;
		updateService.Register(this);
	}

	public void Update() {
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			ISelectableObj selectable = _physics.Raycast<ISelectableObj>(_input.GetWorldMousePosition(), Vector2.zero);
			if (selectable != null && selectable != Selected) {
				SetSelected(selectable);
			}
			else {
				Selected = null;
			}

		}
	}
	
	private void SetSelected(ISelectableObj selectable) {
		Selected = selectable;
	}
}