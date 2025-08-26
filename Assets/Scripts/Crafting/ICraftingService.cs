public interface ICraftingService : IService {
    public CraftingStation GetCraftingStationWithJob();
    public CraftingStation GetCraftingStationWithAvailableCrafting(Race race);
    public void AddCraftingStation(CraftingStation craftingStation);
}