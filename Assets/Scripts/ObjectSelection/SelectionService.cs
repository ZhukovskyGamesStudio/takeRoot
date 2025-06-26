using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SelectionService : ISelectionService, IUpdatable
{
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	public Selectable Selected { get; private set; }
	public bool IsEnabled { get; set; }

	public SelectionService(IInputService inputService, IPhysicsService physics, IUpdateService updateService) {
		_input = inputService;
		_physics = physics;
		updateService.Register(this);
	}

	public void Update() {
		if (!IsEnabled) return;
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			Selectable selectable = _physics.Raycast<Selectable>(_input.GetWorldMousePosition(), Vector2.zero);
			if (selectable != null && selectable != Selected) {
				SetSelected(selectable);
			}
			else {
				//Selected = null;
			}

		}
	}
	
	private void SetSelected(Selectable selectable) {
		Selected = selectable;
	}
}