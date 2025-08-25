using System;

public class ResourcesViewPresenter : IUpdatable, IDisposable {
    private readonly ResourcesView _view;
    private readonly IResourceManager _resourceManager;
    private readonly IUpdateService _updateService;

    public ResourcesViewPresenter(ResourcesView view, IResourceManager resourceManager, IUpdateService updateService) {
        _view = view;
        _resourceManager = resourceManager;
        _updateService = updateService;

        updateService.Register(this);
    }

    public void Update() {
        _view.SetData(_resourceManager.TotalResources());
    }

    public void Dispose() {
        _updateService?.Unregister(this);
    }
}