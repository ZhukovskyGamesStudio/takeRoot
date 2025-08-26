using System.Linq;
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
        if (_tacticalCommandPanel.SelectedTacticalCommand == TacticalCommand.RoundAttack &&
            ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.TakenTacticalCommand == null) {
            TryAddTacticalCommandFromMouseClick(TacticalCommand.RoundAttack);
        }

        if (EventSystem.current?.IsPointerOverGameObject() == true) {
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            if (ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler &&
                ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.Mode == Mode.Tactical) {
                TryAddTacticalCommandFromMouseClick(TacticalCommand.Move);
                return;
            }
        }

        if (Input.GetMouseButtonDown(0) && _tacticalCommandPanel.SelectedTacticalCommand != TacticalCommand.None) {
            if (ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler) {
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
        if (ObsoleteCoreEntryPoint.Instance.MyRace() != _race) {
            return;
        }

        TacticalInteractable interactable = ObsoleteCoreEntryPoint.SelectionManager.TacticalInteractable as TacticalInteractable;
        if (tacticalCommand == TacticalCommand.RoundAttack) {
            interactable = ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.GetEcsComponent<TacticalInteractable>();

            //Выключает тугл RoundAttack чтобы избежать повторных добавлений команды
            TacticalCommandToggle toggle = _tacticalCommandPanel.GetComponentsInChildren<TacticalCommandToggle>()
                .FirstOrDefault(t => t.TacticalCommand == TacticalCommand.RoundAttack);
            if (toggle != null) {
                toggle.OnValueChanged(false);
            }
        }

        if (tacticalCommand == TacticalCommand.Cancel) {
            //RemoveCommand();
        }

        TacticalCommandData data = new();
        data.TacticalCommandType = tacticalCommand;
        if (interactable == null && tacticalCommand != TacticalCommand.Move) {
            return;
        }

        if (interactable == null) {
            data.TacticalInteractable = null;

            Vector2Int position = GetFloorCell();
            data.TargetPosition = position;
            Vector2Int mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2Int();
            data.TargetPoint = new Point(mousePosition.x, mousePosition.y, ObsoleteCoreEntryPoint.LayerManager.currentGlobalLayer);
        } else {
            if (!interactable.CanBeCommanded(tacticalCommand)) {
                return;
            }

            if (tacticalCommand == TacticalCommand.Merge && interactable.TryGetComponent(out Settler settler)) {
                if (settler.SettlerData.Race == ObsoleteCoreEntryPoint.Instance.MyRace()) {
                    return;
                }

                if (settler.SettlerData._mode == Mode.Planning) {
                    return;
                }
            }

            if (tacticalCommand == TacticalCommand.Equip && interactable.TryGetComponent(out TacticalEquippable equippable)) {
                EquipmentType equipType = equippable.GetEquipmentType();
                if (!ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.SettlerData.PossibleEquipment.Contains(equipType)) {
                    return;
                }
            }

            data.TacticalInteractable = interactable;
            data.TargetPosition = interactable.GetInteractableCell;
            interactable.AssignCommand(data);
        }

        AddCommand(data);
        ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.SetTacticalCommand(data);
    }

    private void AddCommand(TacticalCommandData data) {
        _currentCommand = data;
        if (data.PlannedCommandView == null && data.TacticalInteractable != null && data.TacticalCommandType != TacticalCommand.RoundAttack) {
            PlannedCommandView commandView = Instantiate(_plannedCommandView);
            commandView.Init(data.TacticalCommandType, data.TacticalInteractable.Gridable);
            data.PlannedCommandView = commandView;
        }

        data.TriggerCancel += RemoveCommand;
    }

    private void RemoveCommand() {
        if (_currentCommand != null) {
            if (_currentCommand.PlannedCommandView != null) {
                _currentCommand.PlannedCommandView.Release();
            }

            _currentCommand = null;
            ObsoleteCoreEntryPoint.SettlersSelectionManager.SelectedSettler.ClearTacticalCommand();
        }
    }

    private Vector2Int GetFloorCell() {
        Vector2 mousePosition = Vector3.one / 2f + Camera.main.ScreenToWorldPoint(Input.mousePosition);
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