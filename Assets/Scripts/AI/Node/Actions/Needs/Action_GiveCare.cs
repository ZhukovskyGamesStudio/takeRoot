namespace AI.Node.Jobs {
    public class Action_GiveCare : BTNode {
        private readonly Settler _settler;

        public Action_GiveCare(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            var station = _settler.Data.targets.CareStation;
            if (station.IsFree || !station.CareSettlerReady) {
                _settler.CareGiver.Cancel();
                station.ReleaseCaregiverSettler();
                return _state = BTNodeState.Success;
            }

            _settler.CareGiver.Care(station);
            return _state = BTNodeState.Running;
        }
    }
}