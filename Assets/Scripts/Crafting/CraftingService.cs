using System;
using System.Collections.Generic;
using System.Linq;

public class CraftingService : ICraftingService {
    private List<CraftingStation> _craftingStations = new();

    public CraftingStation GetCraftingStationWithJob() {
        foreach (CraftingStation craftingStation in _craftingStations) { //TODO: change crafting station pick
            if (craftingStation.HaulInteractPos == null) continue;
            var type = craftingStation.GetRequiredResource();
            if (type != ResourceType.None)
                return craftingStation;
        }
        return null;
    }

    public CraftingStation GetCraftingStationWithAvailableCrafting(Race race) {
        foreach (CraftingStation station in _craftingStations) {
            if (!station.CanCraft()) continue;
            if (station.Crafters.ContainsKey(race) && station.Crafters[race] == null) {
                return station;
            }
        }
        return null;
    }

    public void AddCraftingStation(CraftingStation craftingStation) {
        _craftingStations.Add(craftingStation);
    }
}