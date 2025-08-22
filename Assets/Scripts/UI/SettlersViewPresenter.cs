using System;

public class SettlersViewPresenter : IUpdatable, IDisposable {
    private AvatarsView _avatarsView;
    private ISettlersService _service;
    private IRaceService _raceService;
    private IUpdateService _updateService;

    public SettlersViewPresenter(AvatarsView view, ISettlersService settlersService, IRaceService raceService, IUpdateService updateService) {
        _avatarsView = view;
        _service = settlersService;
        _raceService = raceService;
        _updateService = updateService;
        
        _avatarsView.InitSettlers(_service.MySettlers(_raceService.MyRace()));
        _updateService.Register(this);
    }

    public void Update() {
        _avatarsView.UpdateSettlers();
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }
}