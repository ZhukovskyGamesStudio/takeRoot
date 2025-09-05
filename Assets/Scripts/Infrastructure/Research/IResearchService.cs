using System;
using System.Collections.Generic;

public interface IResearchService : IService {
    public Action OnResearchFinished { get; set; }
    public bool WasResearched(Research research);
    public Dictionary<Research, ResearchData> GetInitResearchData();
    public ResearchSaveData GetResearchData();
    public void AddResearchPoints(int points);
    public void SelectResearch(Research research);
    public void RegisterResearchStation(ResearchStation researchStation);
    public void UnregisterResearchStation(ResearchStation researchStation);
    public ResearchStation GetResearchStationWithResearch(Race race);
    public void UnlockAllResearches();
}