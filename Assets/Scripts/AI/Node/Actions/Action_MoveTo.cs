using UnityEngine;

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
           //if (target.InteractPosition == null) {
           //    return BTNodeState.Failure;
           //}
            //if (!_settler.Mover.HasPath(target.InteractPosition.Value)) {
            //    return _state = BTNodeState.Failure;
            //}
            if (_settler.Mover.IsMoving) {
                return BTNodeState.Running;
            }
            
            Vector3? interactPosition = null;
            foreach (var position in target.InteractPositions) {
                if (_settler.Mover.HasPath(position)) {
                    interactPosition = position;
                    break;
                }
            }
            if (interactPosition == null) {
                return BTNodeState.Failure;
            }

            if (_settler.Mover.IsAtPosition(interactPosition.Value)) {
                return _state = BTNodeState.Success;
            }

            _settler.Mover.MoveTo(interactPosition.Value, _settler.WorkerAnimator);
            return _state = BTNodeState.Running;
        }
    }
}