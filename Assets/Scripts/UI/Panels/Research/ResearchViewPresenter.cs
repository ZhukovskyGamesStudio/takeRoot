public class ResearchViewPresenter {
    private readonly ResearchPanelView _view;
    private readonly IResearchService _service;

    public ResearchViewPresenter(ResearchPanelView view, IResearchService service) {
        _view = view;
        _service = service;

        _view.InitData(service.GetInitResearchData());
    }

    public void UpdateData() {
        _view.UpdateData(_service.GetResearchData());
    }
}