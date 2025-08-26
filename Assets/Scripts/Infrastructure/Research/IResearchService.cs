using System.Collections.Generic;

public interface IResearchService : IService {
    public Dictionary<Research, ResearchData> GetInitResearchData();
    public ResearchSaveData GetResearchData();
}
