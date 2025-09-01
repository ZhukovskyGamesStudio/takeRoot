namespace AI.Node.Jobs {
    public class Action_MoveTo : BTNode {
        private Settler _settler;
        private readonly bool _useSubsequentTarget;

        public Action_MoveTo(Settler settler, bool useSubsequentTarget = false) {
            _settler = settler;
            _useSubsequentTarget = useSubsequentTarget;
        }

        public override BTNodeState Evaluate() {
            CommandTarget target = _useSubsequentTarget ? _settler.Data.subsequentTarget : _settler.Data.currTarget;
            
            //TODO поселенцы застревают т.к. не могут сломать препятствия сверху вниз
            //мега крит, надо починить!!!
            if (!_settler.Mover.HasPath(target.InteractPosition.position)) {
                return _state = BTNodeState.Failure;
            }

            if (_settler.Mover.IsAtPosition(target.InteractPosition.position)) {
                return _state = BTNodeState.Success;
            }

            _settler.Mover.MoveTo(target.InteractPosition.position, _settler.WorkerAnimator);
            return _state = BTNodeState.Running;
        }
    }
}