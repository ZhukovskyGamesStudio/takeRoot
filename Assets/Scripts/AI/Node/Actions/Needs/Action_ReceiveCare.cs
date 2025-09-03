namespace AI.Node.Jobs {
    public class Action_ReceiveCare : BTNode {
        private Settler _settler;

        public Action_ReceiveCare(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            if (!_settler.Data.needs.CareData.isTakingCareOf) {
                _settler.TeleportToPos(_settler.Data.needs.CareData.careStation.CarePos.position);
                _settler.Data.needs.CareData.isTakingCareOf = true;
                _settler.Data.needs.CareData.careStation.CareSettlerReady = true;
                _settler.StartReceiveCare();
            }

            if (_settler.Data.needs.CareData.HighCare) {
                _settler.TeleportToPos(_settler.Data.needs.CareData.careStation.NearPos.position);
                _settler.StopReceivingCare();
                _settler.Data.needs.CareData.careStation.ReleaseCareSettler();
                return _state = BTNodeState.Success;
            }

            return _state = BTNodeState.Running;
        }
    }
}