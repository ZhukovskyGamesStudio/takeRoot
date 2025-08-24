namespace AI.Node.Jobs {
	public class Action_PickupResource : BTNode {
		private readonly Settler _settler;

		public Action_PickupResource(Settler settler) {
			_settler = settler;
		}
		public override BTNodeState Evaluate() {
			//Mock
			var resource = _settler.Data.craftingTransport.resourceToHaul;
			resource.transform.SetParent(_settler.transform);
			_settler.Data.craftingTransport.isHoldingResource = true;
			_settler.Data.curMovePos = _settler.Data.craftingTransport.craftingStation.transform.position;
			return BTNodeState.Success;
		}
	}
}