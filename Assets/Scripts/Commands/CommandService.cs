using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandService : ICommandService, IUpdatable {
	
	private readonly Dictionary<int, ICommand> _commands;
	
	private readonly IGameFactory _factory;
	private readonly IUpdateService _update;

	public CommandService(IGameFactory factory, IUpdateService update) {
		_factory = factory;
		_commands = new Dictionary<int, ICommand>();
		_update = update;
		_update.Register(this);
	}
	
	public void HandleCommandRequest<TParams>(TParams commandParams) where TParams : ICommandParams{
		var command = _factory.CreateCommand(commandParams);
		_commands.Add(command.Id, command);
	}

	public void Update() {
		//TODO: Update statuses of commands or params
		foreach (var (id, command) in _commands.ToList()) {
			//TODO: Make complex command state handle
			if (command.IsCompleted) {
				_commands.Remove(id);
				continue;
			}

			command.Execute();
		}
	}

	private bool CanExecute(CommandType command, ICommandTarget target) {
		return (target.CommandCapabilities & command) == command;
	}
}