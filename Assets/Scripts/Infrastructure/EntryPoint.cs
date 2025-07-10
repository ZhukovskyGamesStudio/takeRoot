using System;
using CodeBase.Services;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
	[SerializeField]private CommandView _commandView;
	
	private ServiceLocator _services;
	
	private void Awake() {
		var updateService = GetComponent<IUpdateService>();
		var coroutineRunner = GetComponent<ICoroutineRunner>();
		var map = GetComponent<MapFromSceneObjects>();
		var loader = new ServiceLocatorLoader_Main(updateService, coroutineRunner, map);
		
		loader.RegisterServices();
		_services = ServiceLocator.Container;

		_services.Single<IDataProvider>().WorldResourcesData = new WorldResourcesData();
		_services.Single<IDataProvider>().CreaturesData = new CreaturesData();
		
		var commandPresentation = new CommandPresenter();
		commandPresentation.Init(_commandView, _services.Single<IJobCommandsInputHandlerService>());
	}
}