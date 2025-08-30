using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_PickRandomPos : BTNode {
        private readonly Settler _settler;
        private int _range = 1;

        public Action_PickRandomPos(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.IdleMoveTimer += Time.deltaTime;
            if (_settler.Data.IdleMoveTimer < _settler.Data.IdleMoveCooldown) {
                return BTNodeState.Failure;
            }
            Vector3 offset = new(Random.Range(-_range, _range), Random.Range(-_range, _range));
            Vector3 pos = _settler.transform.position + offset;
            if (_settler.Mover.HasPath(pos)) {
                _settler.Data.curMovePos = pos;
                _settler.Data.HasMovePos = true;
                _settler.Data.IsIdle = true;
                return _state = BTNodeState.Success;
            }
            return _state = BTNodeState.Failure;
        }
    }
}