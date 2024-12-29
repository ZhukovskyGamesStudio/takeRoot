using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandsManager : MonoBehaviour {
    public static CommandsManager Instance;

    [SerializeField]
    private List<CommandData> _plannedCommands = new List<CommandData>();

    private List<CommandData> _untakenCommands = new List<CommandData>();
    private List<CommandData> _takenCommands = new List<CommandData>();

    [SerializeField]
    private CommandsPanel _commandsPanel;

    private List<Chamomile> _settlers;

    private void Awake() {
        Instance = this;
        _plannedCommands = new List<CommandData>();
    }

    private void Start() {
        _settlers = SettlersManager.Instance.Settlers;
    }

    private void AddCommand(CommandData data) {
        _plannedCommands.Add(data);
        _untakenCommands.Add(data);
    }

    private void RemoveCommand(CommandData data) {
        _plannedCommands.Remove(data);
        if (!_takenCommands.Contains(data)) {
            return;
        }

        _takenCommands.Remove(data);
        data.Settler.ClearCommand();
        data.Settler = null;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _commandsPanel.SelectedCommand != Command.None) {
            TryAddCommand(_commandsPanel.SelectedCommand);
        }

        if (_untakenCommands.Count == 0) {
            return;
        }

        foreach (Chamomile settler in _settlers) {
            if (settler.TakenCommand != null) {
                continue;
            }

            CommandData nextCommand = _untakenCommands.First();
            nextCommand.Settler = settler;
            _untakenCommands.Remove(nextCommand);
            _takenCommands.Add(nextCommand);
            settler.SetCommand(  nextCommand);
        }
    }

    private void TryAddCommand(Command command) {
        InteractableObject interactable = SelectionManager.Instance.InteractableObject;
        if (interactable == null) {
            return;
        }

        if (!interactable.CanBeCommanded(command)) {
            return;
        }

        if (command == Command.Cancel) {
            RemoveCommand(interactable.CommandToExecute);
            interactable.CancelCommand();
            return;
        }

        CommandData data = new CommandData() {
            CommandType = command,
            InteractableObject = interactable
        };
        AddCommand(data);
        interactable.AssignCommand(data);
    }

    public void PerformedCommand(CommandData data) {
        data.InteractableObject.ExecuteCommand();
        RemoveCommand(data);
    }
}