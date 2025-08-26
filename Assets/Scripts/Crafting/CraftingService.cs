using System.Collections.Generic;
using System.Linq;

public class CraftingService : ICraftingService {
    private List<CraftingStation> _craftingStations = new();

    public CraftingStation GetCraftingStationWithJob() {
        foreach (CraftingStation craftingStation in _craftingStations) { //TODO: change crafting station pick
            var type = craftingStation.GetRequiredResource();
            if (type != ResourceType.None)
                return craftingStation;
        }
        return null;
    }

    public void AddCraftingStation(CraftingStation craftingStation) {
        _craftingStations.Add(craftingStation);
    }
}