using System;
using CodeBase.Services;
using GameResources;
using UnityEngine;

public class CoreEntryPoint : EntryPointBase {
    [SerializeField]
    private WorldConfig _worldConfig;

    [SerializeField]
    private ResourcesConfig _resourceConfig;

    [SerializeField]
    private CommandView _commandView;
    
    [SerializeField]
    private InfoBookView _infoBookView;

    [SerializeField]
    private CoreCanvasUi _coreCanvasUi;

    private ServiceLocator _services;

    private void Awake() {
        if (TrySwitchToLoading()) {
            return;
        }

        var updateService = GetComponent<IUpdateService>();
        var coroutineRunner = GetComponent<ICoroutineRunner>();
        var map = GetComponent<MapFromSceneObjects>();
        var loader = new ServiceLocatorLoader_Main(updateService, coroutineRunner, _resourceConfig,_coreCanvasUi, map, _worldConfig);

        loader.RegisterServices();
        _services = ServiceLocator.Container;

        _services.Single<IDataProvider>().WorldResourcesData = new WorldResourcesData();
        _services.Single<IDataProvider>().CreaturesData = new CreaturesData();

        InitPresenters();
    }

    private void InitPresenters() {
        var commandPresenter = new CommandPresenter();
        commandPresenter.Init(_commandView, _services.Single<IJobCommandsInputHandlerService>());

        var selectionPresenter = new SelectionServicePresenter(_infoBookView,_services.Single<IJobCommandsInputHandlerService>(),_services.Single<ISelectionService>());
        
    }
}