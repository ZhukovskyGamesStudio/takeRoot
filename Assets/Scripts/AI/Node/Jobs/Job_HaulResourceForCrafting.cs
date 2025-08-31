using System;

namespace AI.Node.Jobs {
    public class Job_HaulResourceForCrafting : Sequence {
        public Job_HaulResourceForCrafting(Settler settler, ICraftingService craftingService, IResourceManager resources) {
            SettlerData data = settler.Data;

            var clear = new Action_ClearHaulCrafting(settler, resources);

            ConditionalAction move = new ConditionalAction()
                .Do(new Action_MoveToPos(settler))
                .While(() => data.craftingTransport.craftingStation && data.craftingTransport.resourceToHaul);
            Selector pickUp = new Selector()
                .AddChild(new Conditional(() => data.craftingTransport.HasResourceInHands))
                .AddChild(new Action_PickupResourceForCrafting(settler));

            ConditionalAction moveToCraftingStation = new ConditionalAction()
                .Do(new Action_MoveToPos(settler))
                .While(() => data.craftingTransport.craftingStation);

            Action_StoreInCraftingStation storeInCraftingStation = new(settler);
            
            var transport = new Sequence()
                .AddChild(new Action_FindCraftingStation(settler, craftingService))
                .AddChild(new Action_ReserveResourceForCrafting(settler, resources))
                .AddChild(move)
                .AddChild(pickUp)
                .AddChild(moveToCraftingStation)
                .AddChild(storeInCraftingStation)
                .AddChild(clear);

            var job = new Selector()
                .AddChild(transport)
                .AddChild(clear);
            
            AddChild(job);
        }
    }
}