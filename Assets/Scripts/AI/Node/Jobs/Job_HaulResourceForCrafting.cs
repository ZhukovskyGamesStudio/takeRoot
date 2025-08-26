using System;

namespace AI.Node.Jobs {
    public class Job_HaulResourceForCrafting : Sequence {
        public Job_HaulResourceForCrafting(Settler settler, ICraftingService craftingService, IResourceManager resources) {
            SettlerData data = settler.Data;
            AddChild(new Action_FindCraftingStation(settler, craftingService));
            AddChild(new Action_ReserveResourceForCrafting(settler, resources));

            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(() => data.craftingTransport.craftingStation && data.craftingTransport.resourceToHaul);
            Selector pickUp = new Selector().AddChild(new Conditional(() => data.craftingTransport.HasResourceInHands))
                .AddChild(new Action_PickupResource(settler));

            ConditionalAction moveToCraftingStation = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(() => data.craftingTransport.craftingStation);

            Action_StoreInCraftingStation storeInCraftingStation = new(settler);
            AddChild(move);
            AddChild(pickUp);
            AddChild(moveToCraftingStation);
            AddChild(storeInCraftingStation);
        }
    }
}