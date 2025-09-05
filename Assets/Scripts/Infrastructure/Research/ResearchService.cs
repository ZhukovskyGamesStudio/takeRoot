using System.Collections.Generic;
using UnityEngine;

public class ResearchService : IResearchService {
    private ResearchSaveData _researchSaveData;
    private ResearchConfig _researchConfig;
    private List<ResearchStation> _researchStations = new List<ResearchStation>();

    private Dictionary<Research, ResearchData> _researchData;

    public ResearchService(IConfigsProvider configsProvider) {
        _researchConfig = configsProvider.ResearchConfig;

        CreateMockResearchData();
        LoadResearchData();
        UpdateResearchable();
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

    public ResearchSaveData GetResearchData() {
        return _researchSaveData;
    }

    public void SelectResearch(Research research) {
        if (_researchSaveData.ResearchProgress[research] == _researchData[research].Price) return;
        if (!_researchData[research].Researchable) return;
        
        _researchSaveData.CurrentResearch = research;
        UpdateStationsData();
    }

    public void AddResearchPoints(int points) {
        if (_researchSaveData.CurrentResearch == Research.None) {
            return;
        }

        Research currentResearch = _researchSaveData.CurrentResearch;
        int resultPoints = _researchSaveData.ResearchProgress[currentResearch] + points;

        if (resultPoints > _researchData[currentResearch].Price) {
            resultPoints = _researchData[currentResearch].Price;
        }

        _researchSaveData.ResearchProgress[currentResearch] = resultPoints;
        UpdateStationsData();
        if (resultPoints == _researchData[currentResearch].Price) {
            _researchSaveData.CurrentResearch = Research.None;
            UpdateResearchable();
        }
    }

    private void UpdateResearchable() {
        foreach (ResearchData research in _researchConfig.Researches) {
            research.Researchable = true;
            foreach (Research requirement in research.Requirements) {
                int progress = _researchSaveData.ResearchProgress[requirement];
                int price = _researchData[requirement].Price;
                
                if (progress != price) {
                    research.Researchable = false;
                    break;
                }
            }
        }
    }

    public void RegisterResearchStation(ResearchStation researchStation) {
        _researchStations.Add(researchStation);
    }

    public void UnregisterResearchStation(ResearchStation researchStation) {
        _researchStations.Remove(researchStation);
    }

    private void UpdateStationsData() {
        foreach (ResearchStation station in _researchStations) {
            station.SetResearchData();
        }
    }

    public void UnlockAllResearches() {
        foreach (var (type, data) in _researchData) {
            SelectResearch(type);
            AddResearchPoints(data.Price);
        }
    }
}