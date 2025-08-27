using System.Collections.Generic;

public class ResearchViewPresenter {
    private readonly ResearchPanelView _view;
    private readonly IResearchService _service;

    private Dictionary<Research, ResearchData> _initResearchData;

    public ResearchViewPresenter(ResearchPanelView view, IResearchService service) {
        _view = view;
        _service = service;
        _initResearchData = _service.GetInitResearchData();
        
        _view.InitData(_initResearchData, service.GetResearchData(), this);
    }

    public void SelectResearch(Research research) {
        _service.SelectResearch(research);
    }
    
    public void AddPoints() {
        _service.AddResearchPoints(100);
    }

    public ResearchData GetResearchData(Research research) {
        return _initResearchData[research];
    }
}