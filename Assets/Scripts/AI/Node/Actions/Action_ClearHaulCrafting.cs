namespace AI.Node.Jobs {
	public class Action_ClearHaulCrafting : BTNode {
		private readonly Settler _settler;
		private readonly IResourceManager _resourceManager;

		public Action_ClearHaulCrafting(Settler settler, IResourceManager resourceManager) {
			_settler = settler;
			_resourceManager = resourceManager;
		}
		public override BTNodeState Evaluate() {
			var transportData = _settler.Data.craftingTransport;
			if (transportData.craftingStation != null) {
				var race = _settler.Data.names.Race;
				if (transportData.craftingStation.Crafters[race] == _settler) {
					transportData.craftingStation.Crafters.Remove(race);
				}
			}
			if (transportData.resourceToHaul != null && transportData.resourceInHands.ResourceType == ResourceType.None) {
				transportData.resourceToHaul.Reserved -= transportData.amountToPick;
			}
			if (transportData.resourceInHands.ResourceType != ResourceType.None) {
				_resourceManager.SpawnResource(
					_settler.transform.position, 
					transportData.resourceInHands.ResourceType,
					transportData.resourceInHands.Amount);
				_settler.ResourceCarrier.DropResource();
			}
			transportData.craftingStation = null;
			transportData.resourceToHaul = null;
			transportData.resourceInHands = ResourceData.Empty;
			transportData.amountToPick = 0;
			return BTNodeState.Failure;
			
		}
	}
}