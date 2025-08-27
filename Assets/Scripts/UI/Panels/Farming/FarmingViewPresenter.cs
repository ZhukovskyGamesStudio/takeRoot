public class FarmingViewPresenter {

    private FarmingPanelView _view;
    private readonly IFarmingService _service;

    public FarmingViewPresenter(FarmingPanelView view, IFarmingService service) {
        _view = view;
        _service = service;
    }
}