using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_HandleNeedsChange : BTNode {
        private Settler_EnergyData Energy => _settler.Data.needs.Value.Energy;
        private Settler_StressData Stress => _settler.Data.needs.Value.StressData;
        private Settler_SatietyData Satiety => _settler.Data.needs.Value.SatietyData;
        private Settler_CareData Care => _settler.Data.needs.Value.CareData;

        private readonly Settler _settler;

        public Action_HandleNeedsChange(Settler settler) {
            _settler = settler;
        }

        private void HandleEnergy() {
            Energy.currentEnergy += Energy.energyChange;
        }

        private void HandleStress() {
            if (_settler.Data.Condition == SettlerCondition.Breakdown) {
                return;
            }

            if (Stress.CanBreakdown) {
                if (Random.value <= Stress.breakdownChance) {
                    _settler.StartBreakdown();
                    return;
                }
            }

            Stress.stressChange = 0;

            if (Satiety.LowSatiety) Stress.stressChange += Stress.lowSatietyStressChange;
            else if (Satiety.HighSatiety) Stress.stressChange += Stress.highSatietyStressChange;

            Stress.currentStress += Stress.stressChange;
        }

        private void HandleSatiety() {
            Satiety.currentSatiety += Satiety.satietyChange;
        }

        private void HandleCare() {
            Care.currentCare += Care.careChange;
        }

        public override BTNodeState Evaluate() {
            _settler.Data.needsUpdateTimer += Time.deltaTime;
            if (_settler.Data.Condition == SettlerCondition.Breakdown) Stress.breakdownTimer += Time.deltaTime;

            if (Settler.GlobalGodmode) return BTNodeState.Success;

            if (_settler.Data.needsUpdateTimer >= _settler.Data.needsUpdateCooldown) {
                _settler.Data.needsUpdateTimer = 0;

                HandleEnergy();
                HandleStress();
                HandleSatiety();
                HandleCare();

                Energy.currentEnergy = Mathf.Clamp(Energy.currentEnergy, 0, Energy.maxEnergy);
                Stress.currentStress = Mathf.Clamp(Stress.currentStress, 0, Stress.maxStress);
                Satiety.currentSatiety = Mathf.Clamp(Satiety.currentSatiety, 0, Satiety.maxSatiety);
                Care.currentCare = Mathf.Clamp(Care.currentCare, 0, Care.maxCare);
            }

            if (Stress.breakdownTimer >= Stress.breakdownDuration) {
                _settler.EndBreakdown();
            }

            if (_settler.Data.needs.Value != null) {
                var v = _settler.Data.needs.Value;
                _settler.UpdateNeedsClientRpc(v.Hp, v.Energy, v.SatietyData, v.CareData, v.StressData);
            } else {
                Debug.Log("Needsvalue is null");
            }

            return _state = BTNodeState.Success;
        }
    }
}