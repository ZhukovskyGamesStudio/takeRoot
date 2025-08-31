namespace AI.Node.Jobs {
	public class Action_Craft : BTNode{
		private readonly Settler _settler;

		public Action_Craft(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			var craftingStation = _settler.Data.targets.CraftingStation;
			if (craftingStation.StationData.CurrentRecipe == null) {
				_settler.Crafter.Cancel();
				craftingStation.Crafters[_settler.Data.names.Race] = null;
				_settler.Data.targets.CraftingStation = null;
				return _state = BTNodeState.Success;
			}
			_settler.Crafter.Craft(craftingStation);
			return _state = BTNodeState.Running;
		}
	}
}