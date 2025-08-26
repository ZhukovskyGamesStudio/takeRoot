using System;
using GameResources;

namespace AI {
    [Serializable]
    public class Settler_TransportForCrafting {
        public Resource resourceToHaul;
        public int amountToPick;
        public CraftingStation craftingStation;

        public ResourceData resourceInHands;
        public bool HasResourceInHands => resourceInHands.ResourceType != ResourceType.None;
    }
}