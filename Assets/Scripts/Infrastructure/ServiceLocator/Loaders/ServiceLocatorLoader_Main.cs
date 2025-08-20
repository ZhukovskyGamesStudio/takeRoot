using System;
using CodeBase.Services;
using GameResources;
using Settlers.Infrastructure;
using UnityEngine;

[Serializable]
public class ServiceLocatorLoader_Main {
	private WorldConfig _worldConfig;
	private ResourcesConfig _resourceConfig;
	
	private readonly MapFromSceneObjects _mapFromSceneObjects;

	private IUpdateService _updateService;
	private ICoroutineRunner _coroutineRunner;
	private readonly ServiceLocator _services;
    
	public ServiceLocatorLoader_Main(IUpdateService updateService, ICoroutineRunner coroutineRunner,
		ResourcesConfig resourceConfig, MapFromSceneObjects mapFromSceneObjects = null, WorldConfig worldConfig = null) {
		_worldConfig = worldConfig;
		_resourceConfig = resourceConfig;
		_mapFromSceneObjects = mapFromSceneObjects;
		_services = ServiceLocator.Container;
		if (coroutineRunner == null)
			Debug.LogError($"The coroutine runner cannot be null.");
		else _coroutineRunner = coroutineRunner;
		if (updateService == null)
			Debug.LogError($"The update service cannot be null.");
		else _updateService = updateService;
	}
	
	public void RegisterServices() {
		var worldState = new WorldState(_worldConfig);
		_services.RegisterSingle<IWorldReader>(worldState);
		_services.RegisterSingle<IWorldWriter>(worldState);
		
		
		_services.RegisterSingle<IAssetProvider>(new AssetProvider());
		_services.RegisterSingle<IDataProvider>(new DataProvider());
		_services.RegisterSingle<IPhysicsService>(new PhysicsService());
		_services.RegisterSingle<IInputService>(new InputService());
		_services.RegisterSingle<IUpdateService>(_updateService);
		_services.RegisterSingle<ICoroutineRunner>(_coroutineRunner);
		_services.RegisterSingle<IPathfindService>(new MockPathfindService());
		_services.RegisterSingle<IIdentifierService>(new IdentifierService());
		_services.RegisterSingle<IAsyncRunner>(new UniTaskAsyncRunner());
		var graph = _mapFromSceneObjects.CreateSimpleGraph();
		_services.RegisterSingle<IPathfindService>(new AStar(graph));
		
		_services.RegisterSingle<IResourceManager>(new ResourcesManager(_resourceConfig));
		_services.RegisterSingle<ICommandService>(new CommandService());
		_services.RegisterSingle<IJobCommandsInputHandlerService>(new JobCommandsInputHandlerService(
			_services.Single<IInputService>(),
			_services.Single<IPhysicsService>(),
			_services.Single<IUpdateService>()));
		_services.RegisterSingle<IWorkerAssigner>(new WorkerAssigner(_services.Single<IUpdateService>(), _services.Single<ICommandService>()));
	}
}