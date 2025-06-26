using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandService : ICommandService, IUpdatable {
	
	private readonly Dictionary<int, ICommand> _commands;
	
	private readonly IGameFactory _factory;
	private readonly IUpdateService _update;
	private readonly ICommandParamsFactory _paramsFactory;
	private readonly IInputService _input;

	public CommandService(IGameFactory factory, IUpdateService update, ICommandParamsFactory paramsFactory, IInputService input) {
		_factory = factory;
		_commands = new Dictionary<int, ICommand>();
		_update = update;
		_paramsFactory = paramsFactory;
		_input = input;
		_update.Register(this);
	}

	public void HandleCommandRequest(CommandType type) {
		ICommandParams cParams = null;
		switch (type) {
			case CommandType.Debug:
				cParams = _paramsFactory.CreateDebugCommandParams();
				if (cParams == null) return;
				CreateCommand((DebugCommandParams)cParams);
				break;
			case CommandType.Move:
				cParams = _paramsFactory.CreateMoveCommandParams();
				if (cParams == null) return;
				CreateCommand((MoveCommandParams)cParams);
				break;
			case CommandType.Destroy :
				cParams = _paramsFactory.CreateDestroyCommandParams();
				CreateCommand((DestroyCommandParams)cParams);
				break;
		};
	}

	private void CreateCommand<TParams>(TParams cParams) where TParams : ICommandParams {
		if (cParams == null) return;
		var command = _factory.CreateCommand(cParams);
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

	private bool CanExecute(CommandType command, CommandTarget target) {
		return (target.CommandCapabilities & command) == command;
	}
}
[Flags][Serializable]
public enum CommandType
{
	None = 0,
	Debug = 1 << 0,
	Move = 1 << 1,
	Destroy = 1 << 2,
}