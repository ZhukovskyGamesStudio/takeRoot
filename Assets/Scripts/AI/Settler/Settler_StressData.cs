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
        [Space]
        public float stressCooldown;
        public float stressTimer;

        public bool CanBreakdown => currentStress >= breakdownStressThreshold;
    }
}