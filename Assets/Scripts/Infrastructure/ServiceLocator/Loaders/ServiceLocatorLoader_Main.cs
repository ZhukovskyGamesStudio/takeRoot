using System;
using CodeBase.Services;
using GameResources;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class ServiceLocatorLoader_Main {
    private BuildingsPanelView _buildingsPanelView;
    private readonly InGameDaynightLightView _globalDaynightLight;
    private readonly NetworkDataHolder _networkDataHolder;

    private ResourcesConfig _resourceConfig;

    private CoreCanvasUi _coreCanvasUi;

    private readonly MapFromSceneObjects _mapFromSceneObjects;

    private IUpdateService _updateService;
    private ICoroutineRunner _coroutineRunner;
    private readonly ServiceLocator _services;

    public ServiceLocatorLoader_Main(IUpdateService updateService, ICoroutineRunner coroutineRunner, 
        CoreCanvasUi coreUI, MapFromSceneObjects mapFromSceneObjects,  BuildingsPanelView buildingsPanelView, InGameDaynightLightView globalDaynightLight, NetworkDataHolder networkDataHolder) {
        _coreCanvasUi = coreUI;
        _buildingsPanelView = buildingsPanelView;
        _globalDaynightLight = globalDaynightLight;
        _networkDataHolder = networkDataHolder;

        _mapFromSceneObjects = mapFromSceneObjects;
        _services = ServiceLocator.Container;
        if (coroutineRunner == null) {
            Debug.LogError($"The coroutine runner cannot be null.");
        } else {
            _coroutineRunner = coroutineRunner;
        }

        if (updateService == null) {
            Debug.LogError($"The update service cannot be null.");
        } else {
            _updateService = updateService;
        }
    }

    public void RegisterServices() {
        _services.RegisterSingle<IConfigsProvider>(new ConfigsProvider());
        
        WorldState worldState = new(_services.Single<IConfigsProvider>());
        _services.RegisterSingle<IWorldReader>(worldState);
        _services.RegisterSingle<IWorldWriter>(worldState);

        _services.RegisterSingle<IAssetProvider>(new AssetProvider());
       
        _services.RegisterSingle<IDataProvider>(new DataProvider());
        _services.RegisterSingle<IPhysicsService>(new PhysicsService());
       
        _services.RegisterSingle<IUpdateService>(_updateService);
        _services.RegisterSingle<INetworkService>(new NetworkService(_networkDataHolder));
        _services.RegisterSingle<IInputService>(new InputService(_services.Single<IUpdateService>()));
        _services.RegisterSingle<ICoroutineRunner>(_coroutineRunner);
        _services.RegisterSingle<IPathfindService>(new MockPathfindService());
        _services.RegisterSingle<IIdentifierService>(new IdentifierService());
        _services.RegisterSingle<IAsyncRunner>(new UniTaskAsyncRunner());
        _services.RegisterSingle<IResearchService>(new ResearchService(_services.Single<IConfigsProvider>()));
        _services.RegisterSingle<ICameraMovementService>(new CameraMovementService(_services.Single<IConfigsProvider>(), _services.Single<IUpdateService>()));
        _services.RegisterSingle<ILevelGenerationService>(new LevelGenerationService(_services.Single<INetworkService>()));
        _services.RegisterSingle<IFarmingService>(new FarmingService(_services.Single<IUpdateService>(), _services.Single<IConfigsProvider>(),
            _services.Single<IInputService>()));
        _services.RegisterSingle<IOverlayService>(new OverlayService());
        _services.RegisterSingle<ITimeScaleService>(new TimeScaleService(_services.Single<IConfigsProvider>()));
        _services.RegisterSingle<IIngameTimeService>(new IngameTimeService(
            _services.Single<IConfigsProvider>(), _services.Single<IUpdateService>(),_globalDaynightLight));
        
        _mapFromSceneObjects.CreateMap();
        SimpleGraph graph = _mapFromSceneObjects.CreateSimpleGraph();
        _services.RegisterSingle<IPathfindService>(new AStar(_mapFromSceneObjects));
        _services.RegisterSingle<IGridService>(new GridService(_mapFromSceneObjects));


        _services.RegisterSingle<IRaceService>(new RaceService(_services.Single<INetworkService>()));

        _services.RegisterSingle<IResourceManager>(new ResourcesManager(_services.Single<IConfigsProvider>(), _services.Single<IGridService>(),_services.Single<INetworkService>()));
        _services.RegisterSingle<ICraftingService>(new CraftingService());
        _services.RegisterSingle<ICommandService>(new CommandService());
        _services.RegisterSingle<IJobCommandsInputHandlerService>(new JobCommandsInputHandlerService(_services.Single<IInputService>(),
            _services.Single<IPhysicsService>(), _services.Single<IUpdateService>(),_services.Single<INetworkService>()));
        _services.RegisterSingle<IWorkerAssigner>(new WorkerAssigner(_services.Single<IUpdateService>(), _services.Single<ICommandService>()));

        _services.RegisterSingle<ISelectionService>(new SelectionService(_services.Single<IInputService>(), _services.Single<IPhysicsService>(),
            _services.Single<IUpdateService>(), _services.Single<ICommandService>()));
        _services.RegisterSingle<ISettlersService>(new SettlersService());

        _services.RegisterSingle<IBuildingService>(new BuildingService(_services.Single<IConfigsProvider>(), _buildingsPanelView,_services.Single<INetworkService>()));
        
        _services.RegisterSingle<IOccurenceService>(
            new OccurenceService(_services.Single<IConfigsProvider>(), _services.Single<IUpdateService>(), _services.Single<ISettlersService>(), _services.Single<IGridService>(), _services.Single<INetworkService>()));
        _services.RegisterSingle<INotificationsService>(new NotificationsService(_services.Single<IUpdateService>(), _services.Single<IOccurenceService>()));
        
        _services.RegisterSingle<ITacticalService>(new TacticalService(
            _services.Single<IGridService>(), _services.Single<ISelectionService>(), _services.Single<IUpdateService>(), _services.Single<IInputService>(), _services.Single<IPhysicsService>(),_services.Single<INetworkService>() ));
        _services.RegisterSingle<IFogOfWarService>(new FogOfWarService(_services.Single<IConfigsProvider>(),_services.Single<IUpdateService>(),_services.Single<ISettlersService>(), _services.Single<INetworkService>()));

    }
}