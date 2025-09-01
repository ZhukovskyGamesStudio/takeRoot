using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_HandleNeedsChange : BTNode {
        private readonly Settler_EnergyData _energyData;
        private readonly Settler_StressData _stressData;
        private readonly Settler_SatietyData _satietyData;
        private readonly Settler_CareData _careData;

        private readonly Settler _settler;

        public Action_HandleNeedsChange(Settler settler) {
            _energyData = settler.Data.needs.Energy;
            _stressData = settler.Data.needs.StressData;
            _satietyData = settler.Data.needs.SatietyData;
            _careData = settler.Data.needs.CareData;
            
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
            
            _stressData.stressChange = 0;
            
            if(_satietyData.LowSatiety) _stressData.stressChange += _stressData.lowSatietyStressChange;
            else if(_satietyData.HighSatiety) _stressData.stressChange += _stressData.highSatietyStressChange;
            
            _stressData.currentStress += _stressData.stressChange;
        }

        private void HandleSatiety() {
            _satietyData.currentSatiety += _satietyData.satietyChange;
        }
        
        private void HandleCare() {
            _careData.currentCare += _careData.careChange;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.needsUpdateTimer += Time.deltaTime;
            if(_settler.Data.Condition == SettlerCondition.Breakdown) _stressData.breakdownTimer += Time.deltaTime;

            if (Settler.GlobalGodmode) return BTNodeState.Success;
            
            if (_settler.Data.needsUpdateTimer >= _settler.Data.needsUpdateCooldown) {
                _settler.Data.needsUpdateTimer = 0;
                
                HandleEnergy();
                HandleStress();
                HandleSatiety();
                HandleCare();
                
                _energyData.currentEnergy = Mathf.Clamp(_energyData.currentEnergy, 0, _energyData.maxEnergy);
                _stressData.currentStress = Mathf.Clamp(_stressData.currentStress, 0, _stressData.maxStress);
                _satietyData.currentSatiety = Mathf.Clamp(_satietyData.currentSatiety, 0, _satietyData.maxSatiety);
                _careData.currentCare = Mathf.Clamp(_careData.currentCare, 0, _careData.maxCare);
            }
            if (_stressData.breakdownTimer >= _stressData.breakdownDuration) {
                _settler.EndBreakdown();
            }

            return _state = BTNodeState.Success;
        }
    }
}