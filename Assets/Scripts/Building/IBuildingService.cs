public interface IBuildingService : IService {
	public BuildingBlueprint GetBuildingBlueprintWithJob();
	public void CreateBuildingBlueprint(BuildingRecipeConfig config);
	public void PlaceBlueprint(BuildingBlueprint blueprint);
	public void CancelBlueprint(BuildingBlueprint blueprint);
}