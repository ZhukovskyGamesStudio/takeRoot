public interface IBuildingService : IService {
	public BuildingBlueprint GetBuildingBlueprintWithJob();
	public void CreateBuilding(BuildingRecipeConfig config);
}