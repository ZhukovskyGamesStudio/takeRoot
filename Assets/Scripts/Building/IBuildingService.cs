public interface IBuildingService : IService {
	public BuildingBlueprint GetBuildingBlueprintWithTransportJob();
	public BuildingBlueprint GetBuildingBlueprintWithBuildJob();
	public void CreateBuildingBlueprint(string buildingName, Race placedByRace);
	public void PlaceBlueprint(BuildingBlueprint blueprint);
	public void CancelBlueprint(BuildingBlueprint blueprint);
	public void Build(BuildingBlueprint blueprint);
}