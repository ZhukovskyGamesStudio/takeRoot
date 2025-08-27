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
            var data = _settler.Data.craftingTransport;
            var craftingStation = _settler.Data.craftingTransport.craftingStation;
            if (_settler.Data.craftingTransport.resourceToHaul != null) {
                return BTNodeState.Success;
            }

            ResourceType type = _settler.Data.craftingTransport.craftingStation.GetRequiredResource();
            if (type == ResourceType.None) {
                _settler.Data.craftingTransport.craftingStation = null;
                return BTNodeState.Failure;
            }
            int amount = craftingStation.stationData.RequiredResources[type] - craftingStation.ReservedRequiredResources[type] - craftingStation.stationData.ResourceStorage[type];
            if (amount <= 0) {
                _settler.Data.craftingTransport.craftingStation = null;
                return BTNodeState.Failure;
            }
            
            Resource resourceOnGround = _resources.FindResourceOnGround(type);
            if (resourceOnGround == null) {
                _settler.Data.craftingTransport.craftingStation = null;
                return BTNodeState.Failure;
            }
            
            int amountToReserve = Mathf.Min(amount, resourceOnGround.Amount - resourceOnGround.Reserved);
            if (amountToReserve == 0) {
                _settler.Data.craftingTransport.craftingStation = null;
                return BTNodeState.Failure;
            }

            craftingStation.ReservedRequiredResources[type] += amountToReserve;
            data.amountToPick = amountToReserve;
            data.resourceToHaul = resourceOnGround;
            resourceOnGround.Reserved += amountToReserve;
            _settler.Data.curMovePos = resourceOnGround.transform.position; //TODO: fix pos pick
            return BTNodeState.Success;
        }
    }
}