public interface IBuildingService : IService {
	public BuildingBlueprint GetBuildingBlueprintWithTransportJob();
	public void CreateBuildingBlueprint(BuildingRecipeConfig config);
	public void PlaceBlueprint(BuildingBlueprint blueprint);
	public void CancelBlueprint(BuildingBlueprint blueprint);
}