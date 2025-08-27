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

        _services.Single<IDataProvider>().WorldResourcesData = new WorldResourcesData();
        _services.Single<IDataProvider>().CreaturesData = new CreaturesData();

        InitPresenters();
        GenerateLevel();
    }

    private async UniTask GenerateLevel() {
        await _services.Single<ILevelGenerationService>().Generate();
    }

    private void InitPresenters() {
        CommandPresenter commandPresenter = new();
        commandPresenter.Init(_commandView, _services.Single<IJobCommandsInputHandlerService>());

        SelectionServicePresenter selectionPresenter = new(_infoPanelView, _settlerPanel, _services.Single<IJobCommandsInputHandlerService>(),
            _services.Single<ISelectionService>());
        AvatarsViewPresenter avatarsPresenter = new(_coreCanvasUi.AvatarsView, _services.Single<ISettlersService>(),
            _services.Single<IRaceService>(), _services.Single<IUpdateService>());

        PanelsPresenter panelsPresenter = new(_coreCanvasUi.PanelTogglesView, _coreCanvasUi.PanelsView);

        ResourcesViewPresenter resorcesPresenter = new(_coreCanvasUi.ResourcesView, _services.Single<IResourceManager>(),
            _services.Single<IUpdateService>());

        ResearchViewPresenter researchPresenter = new(_coreCanvasUi.ResearchPanelView, _services.Single<IResearchService>());

        FarmingViewPresenter farmingViewPresenter = new(_coreCanvasUi.FarmingPanelView, _services.Single<IFarmingService>());
    }
}