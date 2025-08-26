using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_HandleEnergy : BTNode {
        private readonly Settler _settler;

        public Action_HandleEnergy(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.energy.energyTimer += Time.deltaTime;
            if (_settler.Data.energy.energyTimer >= _settler.Data.energy.energyCooldown) {
                _settler.Data.energy.currentEnergy += _settler.Data.energy.energyChange;
                _settler.Data.energy.energyTimer = 0;
            }

            return _state = BTNodeState.Success;
        }
    }
}