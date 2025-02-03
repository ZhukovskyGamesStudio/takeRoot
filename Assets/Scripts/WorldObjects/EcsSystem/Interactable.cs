using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : ECSComponent, ISelectable {
    [SerializeField]
    private Vector2Int _interactableShift;

    private readonly List<Command> _availableCommands = new List<Command>() { };
    public Func<InfoBookData> GetInfoFunc;

    public Action<Command> OnCommandPerformed, OnCommandCanceled;

    public Gridable Gridable { get; private set; }

    public CommandData CommandToExecute { get; private set; }

    public HashSet<Vector2Int> InteractableCells => Gridable.InteractableCells;
    public Vector2Int GetInteractableCell => Gridable.GetBottomLeftOnGrid + _interactableShift;
    public bool CanSelect { get; set; } = true;

    private void OnMouseEnter() {
        if (CanSelect)
            SelectionManager.Instance.SetSelected(this);
    }

    private void OnMouseExit() {
        if (CanSelect)
            SelectionManager.Instance.TryClearSelected(this);
    }

    public InfoBookData GetInfoData() {
        return GetInfoFunc?.Invoke();
    }

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
        if (CommandToExecute == null) {
            return;
        }

        OnCommandCanceled?.Invoke(CommandToExecute.CommandType);
        CommandToExecute = null;
    }

    public void ExecuteCommand() {
        if (CommandToExecute == null) {
            return;
        }

        OnCommandPerformed?.Invoke(CommandToExecute.CommandType);
    }

    public void OnDestroyed() {
        _availableCommands.Clear();
        CommandToExecute?.TriggerCancel?.Invoke();
        CancelCommand();
        Destroy(gameObject);
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) {
        Gridable = entity.GetEcsComponent<Gridable>();
    }
}