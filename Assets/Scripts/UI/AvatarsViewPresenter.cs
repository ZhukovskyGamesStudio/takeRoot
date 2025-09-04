using System;
using UniRx;

public class AvatarsViewPresenter : IUpdatable, IDisposable {
    private AvatarsView _avatarsView;
    private ISettlersService _service;
    private IRaceService _raceService;
    private IUpdateService _updateService;
    private ISelectionService _selectionService;

    public AvatarsViewPresenter(AvatarsView view, ISettlersService settlersService, IRaceService raceService, IUpdateService updateService,
        ISelectionService selectionService) {
        _avatarsView = view;
        _service = settlersService;
        _raceService = raceService;
        _updateService = updateService;
        _selectionService = selectionService;

        _raceService.RaceReactive.Subscribe(OnRaceChangeFromAdmin);

        _avatarsView.InitSettlers(_service.MySettlers(_raceService.RaceReactive.Value), _selectionService.SelectSettler);
        _updateService.Register(this);
    }

    private void OnRaceChangeFromAdmin(Race observer) {
        _avatarsView.InitSettlers(_service.MySettlers(observer), _selectionService.SelectSettler);
    }

    public void Update() {
        _avatarsView.UpdateSettlers();
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }
}