using CodeBase.Services;
using Cysharp.Threading.Tasks;
using GameResources;
using Settlers.Building;
using UnityEngine;

public class CoreEntryPoint : EntryPointBase {
    [Header("Configs"), SerializeField]
    private WorldConfig _worldConfig;

    [SerializeField]
    private BuildingsPanelView _buildingsPanelView;

    [SerializeField]
    private BuildingsConfig _buildingsConfig;

    [SerializeField]
    private ResearchConfig _researchConfig;

    [SerializeField]
    private ResourcesConfig _resourceConfig;

    [SerializeField]
    private CameraMovementConfig _cameraMovementConfig;

    [Header("Views"), SerializeField]
    private CommandView _commandView;

    [SerializeField]
    private InfoPanelView _infoPanelView;

    [SerializeField]
    private SettlerInfoPanel _settlerPanel;

    [Space, SerializeField]
    private CoreCanvasUi _coreCanvasUi;

    private ServiceLocator _services;

    private void Awake() {
        if (TrySwitchToLoading()) {
            return;
        }

        IUpdateService updateService = GetComponent<IUpdateService>();
        ICoroutineRunner coroutineRunner = GetComponent<ICoroutineRunner>();
        MapFromSceneObjects map = GetComponent<MapFromSceneObjects>();
        ServiceLocatorLoader_Main loader = new(updateService, coroutineRunner, _resourceConfig, _coreCanvasUi, map, _worldConfig,
            _researchConfig, _cameraMovementConfig, _buildingsConfig, _buildingsPanelView);

        loader.RegisterServices();
        _services = ServiceLocator.Container;

        Single<IDataProvider>().WorldResourcesData = new WorldResourcesData();
        Single<IDataProvider>().CreaturesData = new CreaturesData();

        InitPresenters();
        GenerateLevel();
    }

    private async UniTask GenerateLevel() {
        await Single<ILevelGenerationService>().Generate();
    }

    private void InitPresenters() {
        CommandPresenter commandPresenter = new(_commandView, Single<IJobCommandsInputHandlerService>());

        OverlaysPresenter overlaysPresenter = new OverlaysPresenter(_coreCanvasUi.OverlaysView, Single<IOverlayService>());

        SelectionServicePresenter selectionPresenter = new(_infoPanelView, _settlerPanel, Single<IJobCommandsInputHandlerService>(),
            Single<ISelectionService>(), Single<IUpdateService>());
        AvatarsViewPresenter avatarsPresenter = new(_coreCanvasUi.AvatarsView, Single<ISettlersService>(), Single<IRaceService>(),
            Single<IUpdateService>());

        PanelsPresenter panelsPresenter = new(_coreCanvasUi.PanelTogglesView, _coreCanvasUi.PanelsView);

        ResourcesViewPresenter resorcesPresenter = new(_coreCanvasUi.ResourcesView, Single<IResourceManager>(), Single<IUpdateService>());

        ResearchViewPresenter researchPresenter = new(_coreCanvasUi.ResearchPanelView, Single<IResearchService>());

        FarmingViewPresenter farmingPresenter = new(_coreCanvasUi.FarmingPanelView, Single<IFarmingService>());
        
        NotificationsPresenter notificationsPresenter = new(_coreCanvasUi.NotificationsView, Single<INotificationsService>());
    }

    private TService Single<TService>() where TService : IService {
        return _services.Single<TService>();
    }
}