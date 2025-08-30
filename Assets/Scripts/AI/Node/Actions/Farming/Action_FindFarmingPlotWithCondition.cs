using System;
using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_FindFarmingPlotWithCondition : BTNode {
        private readonly Settler _settler;
        private readonly IFarmingService _farmingService;
        private readonly Func< FarmingPlot,bool> _condition;

        public Action_FindFarmingPlotWithCondition(Settler settler, IFarmingService farmingService, Func< FarmingPlot,bool> condition) {
            _settler = settler;
            _farmingService = farmingService;
            _condition = condition;
        }

        public override BTNodeState Evaluate() {
            var farmingPlot = _farmingService.FarmingPlots.FirstOrDefault(p => _condition(p) && !p.Farmer);
            if (farmingPlot == null) {
                return _state = BTNodeState.Failure;
            }

            _settler.Data.farming.FarmingPlot = farmingPlot;
            farmingPlot.Farmer = _settler;
            _settler.Data.curMovePos = farmingPlot.transform.position;
            return _state = BTNodeState.Success;
        }
    }
}