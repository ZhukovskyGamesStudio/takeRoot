using System.Collections.Generic;

public class ResearchService : IResearchService {
    private ResearchSaveData _researchSaveData;
    private ResearchConfig _researchConfig;

    private Dictionary<Research, ResearchData> _researchData;
    
    public ResearchService(ResearchConfig researchConfig) {
        _researchConfig = researchConfig;
        
        CreateMockResearchData();
        LoadResearchData();
    }

    private void CreateMockResearchData() {
        //тут можешь создавать рандомные параметры для теста
        _researchSaveData = new ResearchSaveData();
    }

    private void LoadResearchData() {
        _researchData = new Dictionary<Research, ResearchData>();
        
        foreach (ResearchData research in _researchConfig.Researches) {
            _researchData[research.Id] = research;
        }
    }

    public Dictionary<Research, ResearchData> GetInitResearchData() {
        return _researchData;
    }

    public void AddResearchPoints(int points) {
        if (_researchSaveData.CurrentResearch == Research.None) return;

        Research currentResearch = _researchSaveData.CurrentResearch;
        int resultPoints = _researchSaveData.ResearchProgress[currentResearch] + points;

        if (resultPoints > _researchData[currentResearch].Price) resultPoints = _researchData[currentResearch].Price;
        
        _researchSaveData.ResearchProgress[currentResearch] = resultPoints;
    }

    public ResearchSaveData GetResearchData() {
        return _researchSaveData;
    }
}