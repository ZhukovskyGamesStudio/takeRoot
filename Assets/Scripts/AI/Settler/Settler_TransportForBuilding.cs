using System;
using GameResources;

namespace AI {
	[Serializable]
	public class Settler_TransportForBuilding {
		public BuildingBlueprint buildingBlueprint;
		public Resource resourceToHaul;
		public int amountToPick;
		public ResourceData resourceInHands;
		public bool HasResourceInHands => resourceInHands.ResourceType != ResourceType.None;
	}
}