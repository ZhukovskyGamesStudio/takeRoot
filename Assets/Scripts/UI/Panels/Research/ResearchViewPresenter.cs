public class ResearchViewPresenter {
    private readonly ResearchPanelView _view;
    private readonly IResearchService _service;

    public ResearchViewPresenter(ResearchPanelView view, IResearchService service) {
        _view = view;
        _service = service;

        _view.SetData(service.GetResearchData());
    }
}