namespace AI.Node.Jobs {
	public class Action_ClearHaulBuilding : BTNode {
		private readonly Settler _settler;
		private readonly IResourceManager _resourceManager;

		public Action_ClearHaulBuilding(Settler settler, IResourceManager resourceManager) {
			_settler = settler;
			_resourceManager = resourceManager;
		}
		public override BTNodeState Evaluate() {
			var transportData = _settler.Data.buildingTransport;
			if (transportData.buildingBlueprint != null) {
				transportData.buildingBlueprint.Builder = null;
			}
			if (transportData.resourceToHaul != null && transportData.resourceInHands.ResourceType == ResourceType.None) {
				transportData.resourceToHaul.Reserved -= transportData.amountToPick;
			}
			if (transportData.resourceInHands.ResourceType != ResourceType.None) {
				_resourceManager.SpawnResource(
					_settler.transform.position, 
					transportData.resourceInHands.ResourceType,
					transportData.resourceInHands.Amount);
			}

			transportData.buildingBlueprint = null;
			transportData.resourceToHaul = null;
			transportData.resourceInHands = ResourceData.Empty;
			return BTNodeState.Failure;
		}
	}
}