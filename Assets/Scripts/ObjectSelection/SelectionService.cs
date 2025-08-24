using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SelectionService : ISelectionService, IUpdatable
{
	private readonly IInputService _input;
	private readonly IPhysicsService _physics;
	private readonly ICommandService _commandService;
	public ReactiveProperty<Selectable> SelectedReactive { get; set; }= new ReactiveProperty<Selectable>(null);
	public ReactiveProperty<bool> IsEnabled { get; set; } = new ReactiveProperty<bool>(true);

	public SelectionService(IInputService inputService, IPhysicsService physics, IUpdateService updateService, ICommandService commandService) {
		_input = inputService;
		_physics = physics;
		_commandService = commandService;
		updateService.Register(this);
	}

	public void Update() {
		if (!IsEnabled.Value) return;
		if (_input.GetMouseButtonDown(MouseButton.Left)) {
			Selectable selectable = _physics.Raycast<Selectable>(_input.GetWorldMousePosition(), Vector2.zero);
			if (selectable != null && selectable != SelectedReactive.Value) {
				Unselect();
				SetSelected(selectable, true);
			}
			else {
				Unselect();
			}

		}
	}

	public void Unselect() {
		if (SelectedReactive.Value == null) return;
		
		SelectedReactive.Value.Selected = false;
		SelectedReactive.Value = null;
	}

	private void SetSelected(Selectable selectable, bool selected) {
		SelectedReactive.Value = selectable;
		selectable.Selected = true;
	}
}