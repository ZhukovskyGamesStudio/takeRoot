using System.Collections.Generic;
using UnityEngine;

public class CommandService : ICommandService, IUpdatable {
	
	private readonly Dictionary<int, ICommand> _commands;
	private Dictionary<int, CommandPerformer> _performers;
	
	public void HandleCommandRequest(CommandType command, Vector2 pos, ICommandTarget target) {
		
		if (!CanExecute(command, target)) return;
		switch (command) {
			case CommandType.Move:
				HandleMoveCommand(pos, target);
				return;
		}
		HandleCommand(command, pos, target);
	}


	private void HandleMoveCommand(Vector2 pos, ICommandTarget target) {
		throw new System.NotImplementedException();
	}

	private void HandleCommand(CommandType command, Vector2 pos, ICommandTarget target) {
		throw new System.NotImplementedException();
	}

	public void Update() {
		throw new System.NotImplementedException();
	}

	private bool CanExecute(CommandType command, ICommandTarget target) {
		return (target.CommandCapabilities & command) == command;
	}
}