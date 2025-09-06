using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchService : IResearchService {
    private ResearchSaveData _researchSaveData;
    private ResearchConfig _researchConfig;
    private List<ResearchStation> _researchStations = new List<ResearchStation>();

    private Dictionary<Research, ResearchData> _researchData;

    public Action OnResearchFinished { get; set; }

    public ResearchService(IConfigsProvider configsProvider) {
        _researchConfig = configsProvider.ResearchConfig;

        CreateMockResearchData();
        LoadResearchData();
        UpdateResearchable();
    }

    public bool IsResearched(Research research) {
        return _researchData[research].Price == _researchSaveData.ResearchProgress[research];
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
        NetworkDataHolder.Instance.ResearchNetworkData.SelectResearch(research);
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
        if (resultPoints == _researchData[currentResearch].Price) {
            NetworkDataHolder.Instance.ResearchNetworkData.SelectResearch(Research.None);
            UpdateResearchable();
            OnResearchFinished?.Invoke();
        }

        UpdateStationsData();
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

    public ResearchStation GetResearchStationWithResearch(Race race) {
        return _researchStations.FirstOrDefault(r => r.HasResearch && (race == Race.Plants ? !r.plantResearcher : !r.robotResearcher));
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