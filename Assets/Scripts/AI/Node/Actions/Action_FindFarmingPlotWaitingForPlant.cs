using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_FindFarmingPlotWaitingForPlant : BTNode {
        private readonly Settler _settler;
        private readonly IFarmingService _farmingService;

        public Action_FindFarmingPlotWaitingForPlant(Settler settler, IFarmingService farmingService) {
            _settler = settler;
            _farmingService = farmingService;
        }

        public override BTNodeState Evaluate() {
            var farmingPlot = _farmingService.FarmingPlots.FirstOrDefault(p=>
                p.PlantState == FarmingPlantState.WaitingForPlanting && !p.Farmer);
            if (farmingPlot == null) {
                return _state = BTNodeState.Failure;
            }

            _settler.Data.farming.FarmingPlot = farmingPlot;
            farmingPlot.Farmer = _settler;
            _settler.Data.curMovePos = farmingPlot.transform.position + Vector3.down;
            return _state = BTNodeState.Success;
        }
    }
}