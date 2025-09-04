using AI.Node;

public class Action_DrinkWater : BTNode {
    private readonly AI.Settler _settler;

    public Action_DrinkWater(AI.Settler settler) {
        _settler = settler;
    }

    public override BTNodeState Evaluate() {
        var target = _settler.Data.targets.Cooler;
        if (!target.HasWater ||_settler.Data.needs.SatietyData.HighSatiety) {
            _settler.StopInteract();
            _settler.Data.targets.Cooler = null;
            _settler.Data.needs.SatietyData.isDrinking = false;
            return _state = BTNodeState.Success;
        }

        if (!_settler.Data.needs.SatietyData.isDrinking) {
            _settler.Data.needs.SatietyData.isDrinking = true;
            _settler.StartInteract();
        }

        _settler.Data.needs.SatietyData.currentSatiety += _settler.Data.targets.Cooler.SatietyChange;
        _settler.Data.targets.Cooler.DecreaseWater();
        return _state = BTNodeState.Running;
    }
}