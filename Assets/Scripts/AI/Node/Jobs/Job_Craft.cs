namespace AI.Node.Jobs {
	public class Job_Craft : Sequence{
		public Job_Craft( Settler settler, ICraftingService craftingService) {
			AddChild(new Action_FindCraftingStationWithCraftJob(settler, craftingService));

			var moveToCraftingStation = new ConditionalAction()
				.Do(new Action_MoveToPos(settler))
				.While(() => settler.Data.crafting.craftingStation &&
				             settler.Data.crafting.craftingStation.CanCraft());

			Action_Craft crafting = new Action_Craft(settler);
			
			AddChild(moveToCraftingStation);
			AddChild(crafting);
		}
	}
}