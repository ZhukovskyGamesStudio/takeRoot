
namespace AI.Node.Jobs {
    public class Action_ClearCareStationFromCaregiver : BTNode {
        private readonly Settler _settler;

        public Action_ClearCareStationFromCaregiver(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.targets.CareStation.ReleaseCaregiverSettler();
            _settler.Data.targets.CareStation = null;
            return _state = BTNodeState.Failure;
        }
    }
}