using System;

namespace AI.Node.Jobs {
    public class Job_HaulResourceForCrafting : Sequence {
        public Job_HaulResourceForCrafting(Settler settler, ICraftingService craftingService, IResourceManager resources) {
            SettlerData data = settler.Data;
            AddChild(new Action_FindCraftingStation(settler, craftingService));
            AddChild(new Action_ReserveResourceForCrafting(settler, resources));

            Func<bool> condition = () => data.craftingTransport.craftingStation && data.craftingTransport.resourceToHaul;

            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);
            Selector pickUp = new Selector().AddChild(new Conditional(() => data.craftingTransport.HasResourceInHands))
                .AddChild(new Action_PickupResource(settler));

            ConditionalAction moveToCraftingStation = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);

            Action_StoreInCraftingStation storeInCraftingStation = new(settler);
            AddChild(move);
            AddChild(pickUp);
            AddChild(moveToCraftingStation);
            AddChild(storeInCraftingStation);
        }
    }
}