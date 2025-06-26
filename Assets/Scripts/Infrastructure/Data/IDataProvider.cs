public interface IDataProvider : IService
{
	public WorldResourcesData WorldResourcesData { get; set; }
	public CreaturesData CreaturesData { get; set; }
}