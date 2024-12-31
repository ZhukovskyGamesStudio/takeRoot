using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    [SerializeField]
    private List<Command> _availableCommands = new List<Command>() { };

    public CommandData CommandToExecute { get; private set; }

    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x - Size.x / 2f + 0.5f),
        Mathf.RoundToInt(transform.position.y - Size.y / 2f + 0.5f));

    public Vector2Int GetInteractableSell => GetCellOnGrid + Vector2Int.down;

    public List<Vector2Int> GetOccupiedPositions() {
        Vector2Int pos = GetCellOnGrid;
        List<Vector2Int> r = new List<Vector2Int>();
        for (int i = 0; i < Size.x; i++) {
            for (int j = 0; j < Size.y; j++) {
                r.Add(new Vector2Int(pos.x + i, pos.y + j));
            }
        }

        return r;
    }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    public Action<Command> OnCommandPerformed;

    public bool CanBeCommanded(Command command) {
        if (CommandToExecute != null && CommandToExecute.CommandType == command) {
            return false;
        }

        if (CommandToExecute != null && command == Command.Cancel) {
            return true;
        }

        return _availableCommands.Contains(command);
    }

    public void RemoveFromPossibleCommands(Command command) {
        if (!_availableCommands.Contains(command)) {
            return;
        }

        _availableCommands.Remove(command);
    }

    public void AddToPossibleCommands(Command command) {
        if (_availableCommands.Contains(command)) {
            return;
        }

        _availableCommands.Add(command);
    }

    public void AssignCommand(CommandData command) {
        CommandToExecute = command;
    }

    public void CancelCommand() {
        CommandToExecute = null;
    }

    public void ExecuteCommand() {
        OnCommandPerformed?.Invoke(CommandToExecute.CommandType);
        CommandToExecute = null;
    }

    private void OnMouseEnter() {
        SelectionManager.Instance.SetSelected(this);
    }

    private void OnMouseExit() {
        SelectionManager.Instance.TryClearSelected(this);
    }
}