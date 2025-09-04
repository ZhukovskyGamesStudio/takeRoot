
using System.Linq;
using AI.Node;
using UnityEngine;

public class Action_TryFindElectricitySource : BTNode {
    private readonly AI.Settler _settler;

    public Action_TryFindElectricitySource(AI.Settler settler) {
        _settler = settler;
    }

    public override BTNodeState Evaluate() {
        ElectricityLevel source = Object.FindObjectsByType<ElectricityLevel>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .FirstOrDefault(b => b.CanDirectlyCharge && b.EnoughToDirectCharge && !b.ConnectedSettler);
        if (source == null) {
            return _state = BTNodeState.Failure;
        }

        source.ConnectedSettler = _settler;
        _settler.Data.targets.ElectricitySource = source;
        _settler.Data.curMovePos = source.DirectChargePos.position;
        return _state = BTNodeState.Success;
    }
}