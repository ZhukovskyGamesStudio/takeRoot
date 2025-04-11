using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandsManager : MonoBehaviour {
    private CommandsPanel _commandsPanel;

    private List<Command> _commandsWithPlannedView;
    private List<CommandData> _plannedCommands = new List<CommandData>();
    private PlannedCommandView _plannedCommandView;
    private Race _race;

    private List<Settler> _settlers;
    private List<CommandData> _takenCommands = new List<CommandData>();
    private List<CommandData> _unreachableCommands = new List<CommandData>();

    private List<CommandData> _untakenCommands = new List<CommandData>();

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _commandsPanel.SelectedCommand != Command.None) {
            TryAddCommandFromMouseClick(_commandsPanel.SelectedCommand);
        }

        SetUnreachableCommands();
        TryGiveUnjobedSettlerCommand();
    }

    public void Init(Race race, CommandsPanel commandsPanel, PlannedCommandView plannedCommandView, List<Command> commandsWithPlannedView) {
        _race = race;
        _commandsPanel = commandsPanel;
        _plannedCommandView = plannedCommandView;
        _plannedCommands = new List<CommandData>();
        _commandsWithPlannedView = commandsWithPlannedView;
    }

    private void Start()
    {
        _settlers = Core.SettlersManager.Settlers.Where(s => s.SettlerData.Race == _race).ToList();
    }

    public void AddCommandManually(CommandData command) {
        AddCommand(command);
    }

    public void RemoveCommandManually(CommandData command) {
        RemoveCommand(command);
    }

    private void AddCommand(CommandData data) {
        _plannedCommands.Add(data);
        _untakenCommands.Add(data);
        if (data.PlannedCommandView == null && _commandsWithPlannedView.Contains(data.CommandType)) {
            PlannedCommandView commandView = Instantiate(_plannedCommandView, data.Interactable.transform);
            commandView.Init(data.CommandType, data.Interactable.Gridable);
            data.PlannedCommandView = commandView;
        }

        data.TriggerCancel += delegate { RemoveCommand(data); };
    }

    private void RemoveCommand(CommandData data) {
        _plannedCommands.Remove(data);
        _untakenCommands.Remove(data);
        if (data.PlannedCommandView != null) {
            data.PlannedCommandView.Release();
        }

        if (!_takenCommands.Contains(data)) {
            return;
        }

        _takenCommands.Remove(data);

        if (data.Settler == null)
            return;
        data.Settler.ClearCommand();
        data.Settler = null;
    }

    public void ReturnCommand(CommandData data) {
        _takenCommands.Remove(data);
        _untakenCommands.Add(data);
        data.Settler.ClearCommand();
        data.Settler = null;
    }

    public void RevokeCommandBecauseItsUnreachable(CommandData data) {
        _takenCommands.Remove(data);
        _untakenCommands.Remove(data);
        _plannedCommands.Remove(data);
        if (data.Settler != null) {
            data.Settler.ClearCommand();
            data.Settler = null;
        }

        _unreachableCommands.Add(data);
        if (data.PlannedCommandView != null) {
            data.PlannedCommandView.SetUnreachableState(true);
        }

        StartCoroutine(TryAddToCommandsAgain(data));
    }

    private IEnumerator TryAddToCommandsAgain(CommandData data) {
        yield return new WaitForSeconds(3);
        if (!_unreachableCommands.Contains(data)) {
            yield break;
        }

        _unreachableCommands.Remove(data);
        data.UnablePerformSettlers.Clear();
        AddCommand(data);
        if (data.PlannedCommandView != null) {
            data.PlannedCommandView.SetUnreachableState(false);
        }
    }

    private void SetUnreachableCommands() {
        foreach (CommandData command in _untakenCommands.ToList()) {
            if (_untakenCommands.First().UnablePerformSettlers.Count == _settlers.Count) {
                RevokeCommandBecauseItsUnreachable(command);
            }
        }
    }

    private void TryGiveUnjobedSettlerCommand() {
        foreach (Settler settler in _settlers) {
            if (_untakenCommands.Count == 0) {
                return;
            }

            if (settler.TakenCommand != null) {
                continue;
            }

            if (settler.Mode == Mode.Tactical) {
                continue;
            }

            CommandData nextCommand = _untakenCommands.First();
            if (nextCommand.UnablePerformSettlers.Contains(settler)) {
                continue;
            }

            if (!settler.SettlerData.AvailableCommands.Contains(nextCommand.CommandType)) {
                continue;
            }

            SetSettlerCommand(settler, nextCommand);
        }
    }

    private void SetSettlerCommand(Settler settler, CommandData nextCommand) {
        nextCommand.Settler = settler;
        _untakenCommands.Remove(nextCommand);
        _takenCommands.Add(nextCommand);
        settler.SetCommand(nextCommand);
    }

    private void TryAddCommandFromMouseClick(Command command) {
        if (Core.Instance.MyRace() != _race) {
            return;
        }

        Interactable interactable = SelectionManager.Instance.Interactable as Interactable;
        if (interactable == null) {
            return;
        }

        if (!interactable.CanBeCommanded(command)) {
            return;
        }

        if (command == Command.Cancel) {
            RemoveCommand(interactable.CommandToExecute);
            interactable.CancelCommand();
            if (_unreachableCommands.Contains(interactable.CommandToExecute)) {
                _unreachableCommands.Remove(interactable.CommandToExecute);
            }

            return;
        }

        CommandData data = new CommandData() {
            CommandType = command,
            Interactable = interactable
        };
        AddCommand(data);
        interactable.AssignCommand(data);
    }

    public void AddSubsequentCommand(CommandData changedCommand) {
        AddCommand(changedCommand);
        SetSettlerCommand(changedCommand.Settler, changedCommand);
    }

    public void PerformedCommand(CommandData data) {
        bool hasSubsequent = data.HasSubsequentCommand;
        data.Interactable.ExecuteCommand(data);
        if (!hasSubsequent) {
            RemoveCommand(data);
        }
    }

    public void SetActivePanel(bool value) {
        _commandsPanel.gameObject.SetActive(value);
        if (value == false) {
            _commandsPanel.ClearSelectedCommand(_commandsPanel.SelectedCommand);
        }
    }
}