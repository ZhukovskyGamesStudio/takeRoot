using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using WorldObjects;

public class TacticalCommandsManager : MonoBehaviour {
    private TacticalCommandData _currentCommand;
    private PlannedCommandView _plannedCommandView;

    private Race _race;

    private TacticalCommandPanel _tacticalCommandPanel;

    private Tilemap _tilemap;

    private void Update() {
        if (EventSystem.current?.IsPointerOverGameObject() == true) {
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            if (SettlersSelectionManager.Instance.SelectedSettler && SettlersSelectionManager.Instance.SelectedSettler.Mode == Mode.Tactical) {
                TryAddTacticalCommandFromMouseClick(TacticalCommand.Move);
                return;
            }
        }

        if (Input.GetMouseButtonDown(0) && _tacticalCommandPanel.SelectedTacticalCommand != TacticalCommand.None) {
            if (SettlersSelectionManager.Instance.SelectedSettler) {
                TryAddTacticalCommandFromMouseClick(_tacticalCommandPanel.SelectedTacticalCommand);
            }
        }
    }

    public void Init(Race race, TacticalCommandPanel commandsPanel, PlannedCommandView plannedCommandView) {
        _race = race;
        _tacticalCommandPanel = commandsPanel;
        _plannedCommandView = plannedCommandView;
        _tilemap = FindAnyObjectByType<Tilemap>();
    }

    private void TryAddTacticalCommandFromMouseClick(TacticalCommand tacticalCommand) {
        TacticalInteractable interactable = SelectionManager.Instance.TacticalInteractable as TacticalInteractable;
        if (tacticalCommand == TacticalCommand.Cancel) {
            //RemoveCommand();
        }

        TacticalCommandData data = new TacticalCommandData();
        data.TacticalCommandType = tacticalCommand;
        if (interactable == null && tacticalCommand != TacticalCommand.Move) {
            return;
        }

        if (interactable == null) {
            data.TacticalInteractable = null;

            var position = GetFloorCell();
            data.TargetPosition = position;
        } else {
            if (!interactable.CanBeCommanded(tacticalCommand)) {
                return;
            }

            data.TacticalInteractable = interactable;
            data.TargetPosition = interactable.GetInteractableCell;
            interactable.AssignCommand(data);
        }

        AddCommand(data);
        SettlersSelectionManager.Instance.SelectedSettler.SetTacticalCommand(data);
    }

    private void AddCommand(TacticalCommandData data) {
        _currentCommand = data;
        if (data.PlannedCommandView == null && data.TacticalInteractable != null) {
            PlannedCommandView commandView = Instantiate(_plannedCommandView);
            commandView.Init(data.TacticalCommandType, data.TacticalInteractable.Gridable);
            data.PlannedCommandView = commandView;
        }

        data.TriggerCancel += RemoveCommand;
    }

    private void RemoveCommand() {
        if (_currentCommand != null) {
            if (_currentCommand.PlannedCommandView != null)
                _currentCommand.PlannedCommandView.Release();

            _currentCommand = null;
            SettlersSelectionManager.Instance.SelectedSettler.ClearTacticalCommand();
        }
    }

    private Vector2Int GetFloorCell() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = _tilemap.WorldToCell(mousePosition);
        return (Vector2Int)gridPosition;
    }

    public void PerformedCommand(TacticalCommandData data) {
        data.TacticalInteractable.ExecuteCommand();
    }

    public void CancelCommand() {
        RemoveCommand();
    }

    public void SetActivePanel(bool value) {
        _tacticalCommandPanel.gameObject.SetActive(value);
        if (value == false) {
            _tacticalCommandPanel.ClearSelectedTacticalCommand(_tacticalCommandPanel.SelectedTacticalCommand);
        }
    }
}