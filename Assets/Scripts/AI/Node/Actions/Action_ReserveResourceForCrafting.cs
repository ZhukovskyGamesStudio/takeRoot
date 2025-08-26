using GameResources;
using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_ReserveResourceForCrafting : BTNode {
        private readonly Settler _settler;
        private readonly IResourceManager _resources;

        public Action_ReserveResourceForCrafting(Settler settler, IResourceManager resources) {
            _settler = settler;
            _resources = resources;
        }

        public override BTNodeState Evaluate() {
            if (_settler.Data.craftingTransport.resourceToHaul != null) {
                return BTNodeState.Success;
            }

            ResourceType type = _settler.Data.craftingTransport.craftingStation.GetRequiredResource();
            if (type == ResourceType.None) {
                return BTNodeState.Failure;
            }

            int amount = _settler.Data.craftingTransport.craftingStation.RequiredResources[type];

            Resource resourceOnGround = _resources.FindResourceOnGround(type);
            if (resourceOnGround == null) {
                return BTNodeState.Failure;
            }

            int amountToReserve = Mathf.Min(amount, resourceOnGround.Amount - resourceOnGround.Reserved);
            _settler.Data.craftingTransport.amountToPick = amountToReserve;
            _settler.Data.craftingTransport.resourceToHaul = resourceOnGround;
            resourceOnGround.Reserved += amountToReserve;
            _settler.Data.curMovePos = resourceOnGround.transform.position; //TODO: fix pos pick
            return BTNodeState.Success;
        }
    }
}