using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_HandleNeedsChange : BTNode {
        private readonly Settler_EnergyData _energyData;
        private readonly Settler_StressData _stressData;

        private readonly Settler _settler;

        public Action_HandleNeedsChange(Settler settler) {
            _energyData = settler.Data.energy;
            _stressData = settler.Data.needs.StressData;
            
            _settler = settler;
        }

        private void HandleEnergy() {
            _energyData.currentEnergy += _energyData.energyChange;
        }

        private void HandleStress() {
            if (_settler.Data.Condition == SettlerCondition.Breakdown) {
                return;
            }
            
            if (_stressData.CanBreakdown) {
                if (Random.value <= _stressData.breakdownChance) {
                    _settler.StartBreakdown();
                    return;
                }
            }
            
            _stressData.currentStress += _stressData.stressChange;
        }
        

        public override BTNodeState Evaluate() {
            _energyData.energyTimer += Time.deltaTime;
            _stressData.stressTimer += Time.deltaTime;
            if(_settler.Data.Condition == SettlerCondition.Breakdown) _stressData.breakdownTimer += Time.deltaTime;
            
            if (_energyData.energyTimer >= _energyData.energyCooldown) {
                HandleEnergy();
                _energyData.currentEnergy = Mathf.Clamp(_energyData.currentEnergy, 0, _energyData.maxEnergy);
                _energyData.energyTimer = 0;
            }

            if (_stressData.stressTimer >= _stressData.stressCooldown) {
                HandleStress();
                _stressData.currentStress = Mathf.Clamp(_stressData.currentStress, 0, _stressData.maxStress);
                _stressData.stressTimer = 0;
            }
            if (_stressData.breakdownTimer >= _stressData.breakdownDuration) {
                _settler.EndBreakdown();
            }


            return _state = BTNodeState.Success;
        }
    }
}