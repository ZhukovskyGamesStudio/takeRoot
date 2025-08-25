public class ResearchService : IResearchService {
    private ResearchSaveData _researchSaveData;

    public ResearchService() {
        CreateMockResearchData();
    }

    private void CreateMockResearchData() {
        //тут можешь создавать рандомные параметры для теста
        _researchSaveData = new ResearchSaveData();
    }

    public ResearchSaveData GetResearchData() {
        return _researchSaveData;
    }
}