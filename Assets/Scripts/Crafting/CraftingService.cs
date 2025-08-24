using System.Collections.Generic;
using System.Linq;

public class CraftingService : ICraftingService {
	private List<CraftingStation> _craftingStations = new List<CraftingStation>();
	
	
	public CraftingStation GetCraftingStationWithJob() {
		return _craftingStations.FirstOrDefault(c => c.RequiredResources.Count != 0);
	}

	public void AddCraftingStation(CraftingStation craftingStation) {
		_craftingStations.Add(craftingStation);
	}
}