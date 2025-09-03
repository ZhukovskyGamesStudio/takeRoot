using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CoreEntryPoint : EntryPointBase {
    [Header("Views"), SerializeField]
    private CommandView _commandView;

    [SerializeField]
    private InfoPanelView _infoPanelView;
    
    [SerializeField]
    private BuildingsPanelView _buildingsPanelView;

    [SerializeField]
    private SettlerInfoPanel _settlerPanel;

    [SerializeField]
    private TimeStatusUI _timeStatusView;

    [Space, SerializeField]
    private CoreCanvasUi _coreCanvasUi;
    
    [SerializeField]
    private InGameDaynightLightView _globalDaynightLight;

    [SerializeField]
    private NetworkDataHolder _networkDataHolderPrefab;
    
    private ServiceLocator _services;

    private void Awake() {
        if (TrySwitchToLoading()) {
            return;
        }

        IUpdateService updateService = GetComponent<IUpdateService>();
        ICoroutineRunner coroutineRunner = GetComponent<ICoroutineRunner>();
        MapFromSceneObjects map = GetComponent<MapFromSceneObjects>();
        ServiceLocatorLoader_Main loader = new(updateService, coroutineRunner, _coreCanvasUi, map, _buildingsPanelView,_globalDaynightLight,_networkDataHolderPrefab);

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
        CoreCanvasUiPresenter coreCanvasUiPresenter = new(_coreCanvasUi, Single<IRaceService>());
        
        CommandPresenter commandPresenter = new(_commandView, Single<IJobCommandsInputHandlerService>());

        OverlaysPresenter overlaysPresenter = new OverlaysPresenter(_coreCanvasUi.OverlaysView, Single<IOverlayService>());

        SelectionServicePresenter selectionPresenter = new(_infoPanelView, _settlerPanel, Single<IJobCommandsInputHandlerService>(),
            Single<ISelectionService>(), Single<IUpdateService>(), Single<INetworkService>());
        AvatarsViewPresenter avatarsPresenter = new(_coreCanvasUi.AvatarsView, Single<ISettlersService>(), Single<IRaceService>(),
            Single<IUpdateService>());

        PanelsPresenter panelsPresenter = new(_coreCanvasUi.PanelTogglesView, _coreCanvasUi.PanelsView, Single<ISelectionService>(), Single<INetworkService>());

        ResourcesViewPresenter resorcesPresenter = new(_coreCanvasUi.ResourcesView, Single<IResourceManager>(), Single<IUpdateService>());

        ResearchViewPresenter researchPresenter = new(_coreCanvasUi.ResearchPanelView, Single<IResearchService>());

        FarmingViewPresenter farmingPresenter = new(_coreCanvasUi.FarmingPanelView, Single<IFarmingService>());
        
        NotificationsPresenter notificationsPresenter = new(_coreCanvasUi.NotificationsView, Single<INotificationsService>());

        TimeStatusPresenter timeStatusPresenter = new(_timeStatusView, Single<ITimeScaleService>(), Single<IIngameTimeService>());
    }

    private TService Single<TService>() where TService : IService {
        return _services.Single<TService>();
    }
}