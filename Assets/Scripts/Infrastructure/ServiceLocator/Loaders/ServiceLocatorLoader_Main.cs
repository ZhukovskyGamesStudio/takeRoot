using System;
using CodeBase.Services;
using UnityEngine;

[Serializable]
public class ServiceLocatorLoader_Main
{
    private IUpdateService _updateService;
    private readonly ServiceLocator _services;
    
    public ServiceLocatorLoader_Main(IUpdateService updateService)
    {
        _services = ServiceLocator.Container;
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
        
        
        _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IDataProvider>()));
        _services.RegisterSingle<ISelectionService>(new SelectionService(_services.Single<IInputService>(), _services.Single<IPhysicsService>(), _services.Single<IUpdateService>()));
    }
}