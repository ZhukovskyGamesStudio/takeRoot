using GameResources;
using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_ReserveResourceForBuilding : BTNode {
		private readonly Settler _settler;
		private readonly IResourceManager _resourceManager;

		public Action_ReserveResourceForBuilding(Settler settler, IResourceManager resourceManager) {
			_settler = settler;
			_resourceManager = resourceManager;
		}
		public override BTNodeState Evaluate() {
			var data = _settler.Data.buildingTransport;
			var blueprint = data.buildingBlueprint;
			if (data.resourceToHaul != null) {
				return BTNodeState.Success;
			}

			ResourceType type = blueprint.GetRequiredResource();
			if (type == ResourceType.None) {
				return BTNodeState.Failure;
			}
			
			int amount = blueprint.RequiredResources[type] - blueprint.ReservedRequiredResources[type] - blueprint.ResourceStorage[type];
			if (amount <= 0) {
				return BTNodeState.Failure;
			}

			Resource resourceOnGround = _resourceManager.FindResourceOnGround(type);
			if (resourceOnGround == null) {
				return BTNodeState.Failure;
			}
			int amountToReserve = Mathf.Min(amount, resourceOnGround.Amount - resourceOnGround.Reserved);
			if (amountToReserve == 0) {
				return BTNodeState.Failure;
			}
			blueprint.ReservedRequiredResources[type] += amountToReserve;
			data.amountToPick = amountToReserve;
			data.resourceToHaul = resourceOnGround;
			resourceOnGround.Reserved += amountToReserve;
			_settler.Data.curMovePos = data.resourceToHaul.transform.position;
			return BTNodeState.Success;
		}
	}
}