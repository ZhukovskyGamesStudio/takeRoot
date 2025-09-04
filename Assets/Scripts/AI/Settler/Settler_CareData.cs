using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_CareData {
        public int maxCare;
        public float currentCare;

        [Space]
        public int lowCareThreshold;

        public int highCareThreshold;

        [Space]
        public float defaultCareChange;
        public float onStationCareChange;
        
        [HideInInspector]
        public float careChange;
        [Space]
        public bool isTakingCareOf;
        
        public bool LowCare => currentCare < lowCareThreshold;
        public bool HighCare => currentCare >= highCareThreshold;
        public float Percentage => (float)currentCare / maxCare;
    }
}