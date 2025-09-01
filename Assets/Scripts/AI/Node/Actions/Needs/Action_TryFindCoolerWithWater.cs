using System.Linq;
using AI.Node;
using UnityEngine;

public class Action_TryFindCoolerWithWater : BTNode {
    private readonly AI.Settler _settler;

    public Action_TryFindCoolerWithWater(AI.Settler settler) {
        _settler = settler;
    }

    public override BTNodeState Evaluate() {
        Cooler coolerWithWater = Object.FindObjectsByType<Cooler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .FirstOrDefault(b => b.HasWater);
        if (coolerWithWater == null) {
            return _state = BTNodeState.Failure;
        }

        _settler.Data.needs.SatietyData.Cooler = coolerWithWater;
        _settler.Data.curMovePos = coolerWithWater.GetComponent<CommandTarget>().InteractPosition.position;
        return _state = BTNodeState.Success;
    }
}