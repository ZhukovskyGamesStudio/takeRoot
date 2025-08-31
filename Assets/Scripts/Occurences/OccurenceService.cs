public class OccurenceService : IOccurenceService, IUpdatable {
    private readonly IUpdateService _updateService;

    public OccurenceService(IConfigsProvider configsProvider, IUpdateService updateService) {
        _updateService = updateService;
        _updateService.Register(this);
    }

    public void Update() {
        
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }
}