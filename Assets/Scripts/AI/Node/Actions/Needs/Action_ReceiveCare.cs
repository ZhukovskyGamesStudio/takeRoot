namespace AI.Node.Jobs {
    public class Action_ReceiveCare : BTNode {
        private Settler _settler;

        public Action_ReceiveCare(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.needs.Value.CareData.isTakingCareOf) {
                _settler.TeleportToPos(_settler.Data.targets.SitOnCareStation.CarePos.position);
                _settler.Data.needs.Value.CareData.isTakingCareOf = true;
                _settler.Data.targets.SitOnCareStation.CareSettlerReady = true;
                _settler.StartReceiveCare();
            }

            if (_settler.Data.needs.Value.CareData.HighCare) {
                _settler.TeleportToPos(_settler.Data.targets.SitOnCareStation.NearPos.position);
                _settler.StopReceivingCare();
                _settler.Data.targets.SitOnCareStation.ReleaseCareSettler();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}