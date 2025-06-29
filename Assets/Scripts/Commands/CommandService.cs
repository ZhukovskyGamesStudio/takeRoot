using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CodeBase.Services;

public class CommandService : ICommandService, IUpdatable {
	
	private readonly Dictionary<int, ICommand> _commands;
	
	private readonly IUpdateService _update;
	private readonly IIdentifierService _identifier;
	private readonly IGameFactory _factory;

	public CommandService(IGameFactory factory, IUpdateService update) {
		_commands = new Dictionary<int, ICommand>();
		_factory = factory;
		_update = update;
		_update.Register(this);
	}

	public void HandleCommandRequest(CommandType type) {
		ICommand command = null;
		switch (type) {
			case CommandType.Debug:
				command = _factory.CreateCommand(CommandType.Debug);
				_commands.Add(command.Id, command);
				break;
			case CommandType.Destroy :
				command = _factory.CreateCommand(CommandType.Destroy);
				_commands.Add(command.Id, command);
				break;
		}
	}
	

	public void Update() {
		foreach (var (id, command) in _commands.ToList()) {
			switch (command.State) {
				case CommandState.Failed:
					CancelCommand(id);
					break;
				case CommandState.Completed:
					CancelCommand(id);
					break;
				case CommandState.NeedResolve:
					command.TryResolve();
					break;
				case CommandState.InProgress:
					command.Execute();
					break;
			}
		}
	}

	public void CancelCommand(int id) {
		if (_commands.TryGetValue(id, out ICommand command)) {
			command.Cancel();
		}
	}
}

[Flags][Serializable]
public enum CommandType
{
	None = 0,
	Debug = 1 << 0,
	Cancel = 1 << 1,
	Move = 1 << 2,
	Destroy = 1 << 3,
}