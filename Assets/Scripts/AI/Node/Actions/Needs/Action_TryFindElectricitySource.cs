
using System.Linq;
using AI.Node;
using UnityEngine;

public class Action_TryFindElectricitySource : BTNode {
    private readonly AI.Settler _settler;

    public Action_TryFindElectricitySource(AI.Settler settler) {
        _settler = settler;
    }

    public override BTNodeState Evaluate() {
        ElectricityLevel coolerWithWater = Object.FindObjectsByType<ElectricityLevel>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .FirstOrDefault(b => b.CanDirectlyCharge && b.EnoughToDirectCharge);
        if (coolerWithWater == null) {
            return _state = BTNodeState.Failure;
        }

        _settler.Data.needs.SatietyData.ElectricitySource = coolerWithWater;
        _settler.Data.curMovePos = coolerWithWater.DirectChargePos.position;
        return _state = BTNodeState.Success;
    }
}