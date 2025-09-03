public class CoreCanvasUiPresenter {
    private readonly CoreCanvasUi _view;
    private readonly IRaceService _raceService;

    public CoreCanvasUiPresenter(CoreCanvasUi view, IRaceService raceService) {
        _view = view;
        _raceService = raceService;
        
        _view.InitRace(_raceService.RaceReactive.Value);
    }
}
