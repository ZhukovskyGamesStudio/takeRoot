using AI.Node;

public class Action_ConsumeElectricity : BTNode {
    private readonly AI.Settler _settler;

    public Action_ConsumeElectricity(AI.Settler settler) {
        _settler = settler;
    }

    public override BTNodeState Evaluate() {
        var target = _settler.Data.needs.SatietyData.ElectricitySource;
        if (!target.HasElectricity ||_settler.Data.needs.SatietyData.HighSatiety) {
            _settler.StopInteract();
            _settler.Data.needs.SatietyData.ElectricitySource = null;
            return _state = BTNodeState.Success;
        }

        if (!_settler.Data.needs.SatietyData.isDrinking) {
            _settler.Data.needs.SatietyData.isDrinking = true;
            _settler.StartInteract();
        }
        
        var energyTransferSpeed = _settler.Data.needs.SatietyData.ElectricitySource.DirectChargeSpeed;
        _settler.Data.needs.SatietyData.currentSatiety += energyTransferSpeed;
        _settler.Data.needs.SatietyData.ElectricitySource.DecreaseElectricity(energyTransferSpeed);
        return _state = BTNodeState.Running;
    }
}