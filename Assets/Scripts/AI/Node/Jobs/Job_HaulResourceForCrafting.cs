using System;

namespace AI.Node.Jobs {
	public class Job_HaulResourceForCrafting : Sequence {
		public Job_HaulResourceForCrafting(Settler settler, ICraftingService craftingService, IResourceManager resources) {
			var data = settler.Data;
			AddChild(new Action_FindCraftingStation(settler, craftingService));
			AddChild(new Action_ReserveResourceForCrafting(settler, resources));
			
			Func<bool> condition = () => data.craftingTransport.craftingStation &&
			                             data.craftingTransport.resourceToHaul;

			var move = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(condition);
			var pickUp = new Selector()
				.AddChild(new Conditional(() => data.craftingTransport.HasResourceInHands))
				.AddChild(new Action_PickupResource(settler));
			
			var moveToCraftingStation = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(condition);

			var storeInCraftingStation = new Action_StoreInCraftingStation(settler); 
			AddChild(move);
			AddChild(pickUp);
			AddChild(moveToCraftingStation);
			AddChild(storeInCraftingStation);
		}
	}
}