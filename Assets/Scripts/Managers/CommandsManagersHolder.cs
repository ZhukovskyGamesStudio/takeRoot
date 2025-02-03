using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CommandsManagersHolder : NetworkBehaviour, IInitableInstance {
    [SerializeField]
    private PlannedCommandView _plannedCommandView;

    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private TacticalCommandPanel _tacticalCommandPanel;

    private Dictionary<Race, CommandsManager> _commandsManagers;
    private Dictionary<Race, TacticalCommandsManager> _tacticalCommandsManagers;

    public static CommandsManagersHolder Instance => Core.CommandsManagersHolder;

    public CommandsManager CommandsManager => _commandsManagers[Core.Instance.MyRace()];
    public TacticalCommandsManager TacticalCommandsManager => _tacticalCommandsManagers[Core.Instance.MyRace()];

    public void Init() {
        Core.CommandsManagersHolder = this;
        CreateManagers();
    }

    public List<Type> GetDependencies() => new() { typeof(SettlersManager), typeof(CoreCanvasUi) };

    private void CreateManagers() {
        _commandsManagers = new Dictionary<Race, CommandsManager> {
            { Race.Plants, gameObject.AddComponent<CommandsManager>() },
            { Race.Robots, gameObject.AddComponent<CommandsManager>() }
        };
        _commandsManagers[Race.Plants].Init(Race.Plants, _commandsPanel, _plannedCommandView);
        _commandsManagers[Race.Robots].Init(Race.Robots, _commandsPanel, _plannedCommandView);

        _tacticalCommandsManagers = new Dictionary<Race, TacticalCommandsManager> {
            { Race.Plants, gameObject.AddComponent<TacticalCommandsManager>() },
            { Race.Robots, gameObject.AddComponent<TacticalCommandsManager>() }
        };
        _tacticalCommandsManagers[Race.Plants].Init(Race.Plants, _tacticalCommandPanel, _plannedCommandView);
        _tacticalCommandsManagers[Race.Robots].Init(Race.Robots, _tacticalCommandPanel, _plannedCommandView);
    }
}