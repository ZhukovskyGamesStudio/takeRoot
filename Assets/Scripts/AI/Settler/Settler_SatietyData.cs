using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_SatietyData {
        public int maxSatiety;
        public float currentSatiety;
        [Space]
        public int lowSatietyThreshold;
        public int highSatietyThreshold;
        [Space]
        public float satietyChange;

        public bool isDrinking;
        
        public bool LowSatiety => currentSatiety < lowSatietyThreshold;
        public bool HighSatiety => currentSatiety >= highSatietyThreshold;
        public float Percentage => (float)currentSatiety / maxSatiety;
    }
}