using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_StressData {
        public int maxStress;
        public int currentStress;
        [Space]
        public int breakdownStressThreshold;
        public float breakdownChance;
        public float breakdownDuration;
        public float breakdownTimer;
        public int stressAfterBreakdown;
        [Space]
        public int stressChange;
        public int lowSatietyStressChange;
        public int highSatietyStressChange;

        public bool CanBreakdown => currentStress >= breakdownStressThreshold;
        public float Percentage => (float)currentStress / maxStress;
    }
}