using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SelectionService : ISelectionService, IUpdatable
{
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly ICommandService _commandService;
	public Selectable Selected { get; private set; }
	public bool IsEnabled { get; set; } = true;

	public SelectionService(IInputService inputService, IPhysicsService physics, IUpdateService updateService, ICommandService commandService) {
		_input = inputService;
		_physics = physics;
		_commandService = commandService;
		updateService.Register(this);
	}

	public void Update() {
		if (!IsEnabled) return;
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			Selectable selectable = _physics.Raycast<Selectable>(_input.GetWorldMousePosition(), Vector2.zero);
			if (selectable != null && selectable != Selected) {
				TryUnselect();
				SetSelected(selectable, true);
			}
			else {
				TryUnselect();
			}

		}
	}

	private void TryUnselect() {
		if (Selected != null) {
			Selected.Selected = false;
				Selected = null;
		}
	}

	private void SetSelected(Selectable selectable, bool selected) {
		Selected = selectable;
		selectable.Selected = true;
	}
}