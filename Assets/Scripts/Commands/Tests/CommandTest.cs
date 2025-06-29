using System;
using CodeBase.Services;
using UnityEngine;

public class CommandTest : MonoBehaviour{
	
	public CommandType commandType;
	private ICommandInputHandlerService _commandInputHandler;
	private IGameFactory _factory;
	private ISelectionService _selectionService;

	private void Start() {
		_commandInputHandler = ServiceLocator.Container.Single<ICommandInputHandlerService>();
		_factory = ServiceLocator.Container.Single<IGameFactory>();
		_selectionService = ServiceLocator.Container.Single<ISelectionService>();
	}

	public void ChangeCommand() {
		_commandInputHandler.PendingCommand.Value = commandType;
		if (commandType == CommandType.None) {
			_selectionService.IsEnabled = true;
		}
		else _selectionService.IsEnabled = false;
	}

	public void Spawn() {
		var settler = _factory.CreateSettler("TestSettler", new Vector3(0, 0, 0));
	}

	public void Dispose() {
		throw new NotImplementedException();
	}
}