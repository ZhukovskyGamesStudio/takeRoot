using System;
using GameResources;

namespace AI {
	[Serializable]
	public class Settler_TransportForCrafting {
		public Resource resourceToHaul;
		public int amountToPick;
		public CraftingStation craftingStation;
		public bool isHoldingResource;
	}
}