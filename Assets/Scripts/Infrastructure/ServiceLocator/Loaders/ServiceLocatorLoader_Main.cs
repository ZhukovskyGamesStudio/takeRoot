using System;
using CodeBase.Services;
using GameResources;
using UnityEngine;

[Serializable]
public class ServiceLocatorLoader_Main {
    private WorldConfig _worldConfig;
    private readonly CameraMovementConfig _cameraMovementConfig;

    private ResourcesConfig _resourceConfig;
    private ResearchConfig _researchConfig;

    private CoreCanvasUi _coreCanvasUi;

    private readonly MapFromSceneObjects _mapFromSceneObjects;

    private IUpdateService _updateService;
    private ICoroutineRunner _coroutineRunner;
    private readonly ServiceLocator _services;

    public ServiceLocatorLoader_Main(IUpdateService updateService, ICoroutineRunner coroutineRunner, ResourcesConfig resourceConfig,
        CoreCanvasUi coreUI, MapFromSceneObjects mapFromSceneObjects, WorldConfig worldConfig, ResearchConfig researchConfig,
        CameraMovementConfig cameraMovementConfig) {
        _coreCanvasUi = coreUI;
        _worldConfig = worldConfig;
        _researchConfig = researchConfig;
        _cameraMovementConfig = cameraMovementConfig;
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
        _services.RegisterSingle<IResearchService>(new ResearchService(_researchConfig));
        _services.RegisterSingle<ICameraMovementService>(new CameraMovementService(_cameraMovementConfig, _services.Single<IUpdateService>()));
        _services.RegisterSingle<ILevelGenerationService>(new LevelGenerationService());

        _mapFromSceneObjects.CreateMap();
        var graph = _mapFromSceneObjects.CreateSimpleGraph();
        _services.RegisterSingle<IPathfindService>(new AStar(_mapFromSceneObjects));
        _services.RegisterSingle<IGridService>(new GridService(_mapFromSceneObjects));

        //TODO setup race from online service
        _services.RegisterSingle<IRaceService>(new RaceService(Race.Plants));

        _services.RegisterSingle<IResourceManager>(new ResourcesManager(_resourceConfig, _services.Single<IGridService>()));
        _services.RegisterSingle<ICraftingService>(new CraftingService());
        _services.RegisterSingle<ICommandService>(new CommandService());
        _services.RegisterSingle<IJobCommandsInputHandlerService>(new JobCommandsInputHandlerService(_services.Single<IInputService>(),
            _services.Single<IPhysicsService>(), _services.Single<IUpdateService>()));
        _services.RegisterSingle<IWorkerAssigner>(new WorkerAssigner(_services.Single<IUpdateService>(), _services.Single<ICommandService>()));

        _services.RegisterSingle<ISelectionService>(new SelectionService(_services.Single<IInputService>(), _services.Single<IPhysicsService>(),
            _services.Single<IUpdateService>(), _services.Single<ICommandService>()));
        _services.RegisterSingle<ISettlersService>(new SettlersService());
    }
}