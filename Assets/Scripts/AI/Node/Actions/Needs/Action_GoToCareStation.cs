namespace AI.Node.Jobs {
    public class Action_GoToCareStation : BTNode {
        private Settler _settler;

        public Action_GoToCareStation(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.curMovePos = _settler.Data.targets.SitOnCareStation.NearPos.position;
            return _state = BTNodeState.Success;
        }
    }
}