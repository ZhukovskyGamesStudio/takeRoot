using System;
using CodeBase.Services;
using UnityEngine;

[Serializable]
public class ServiceLocatorLoader_Main
{

	private IUpdateService _updateService;
	private ICoroutineRunner _coroutineRunner;
	private readonly ServiceLocator _services;
    
	public ServiceLocatorLoader_Main(IUpdateService updateService, ICoroutineRunner coroutineRunner)
	{
		_services = ServiceLocator.Container;
		if (coroutineRunner == null)
			Debug.LogError($"The coroutine runner cannot be null.");
		else _coroutineRunner = coroutineRunner;
		if (updateService == null)
			Debug.LogError($"The update service cannot be null.");
		else _updateService = updateService;
	}
    
    
	public void RegisterServices()
	{
		_services.RegisterSingle<IAssetProvider>(new AssetProvider());
		_services.RegisterSingle<IDataProvider>(new DataProvider());
		_services.RegisterSingle<IPhysicsService>(new PhysicsService());
		_services.RegisterSingle<IInputService>(new InputService());
		_services.RegisterSingle<IUpdateService>(_updateService);
		_services.RegisterSingle<ICoroutineRunner>(_coroutineRunner);
		_services.RegisterSingle<IPathfindService>(new MockPathfindService());
		_services.RegisterSingle<IIdentifierService>(new IdentifierService());
        
		//_services.RegisterSingle<IWorkerService>(new WorkerService());
		
		_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), 
			_services.Single<IDataProvider>(), 
			_services.Single<IInputService>(),
			_services.Single<IPhysicsService>(),
			_services.Single<ICoroutineRunner>(),
			_services.Single<IPathfindService>(),
			_services.Single<IIdentifierService>(),
			_services.Single<IWorkerService>()));
		
		_services.RegisterSingle<ICommandInputHandlerService>(new CommandInputHandlerService(_services.Single<IInputService>(), 
			_services.Single<IPhysicsService>(), _services.Single<ICommandService>(), _services.Single<IUpdateService>()));
		
		
		_services.RegisterSingle<ISelectionService>(new SelectionService(_services.Single<IInputService>(), _services.Single<IPhysicsService>(), 
			_services.Single<IUpdateService>(), _services.Single<ICommandService>()));
		
	}
}