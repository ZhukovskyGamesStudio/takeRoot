public interface ICraftingService : IService {
    public CraftingStation GetCraftingStationWithJob();
    public void AddCraftingStation(CraftingStation craftingStation);
}