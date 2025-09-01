using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_CareData {
        public int maxCare;
        public int currentCare;

        [Space]
        public int lowCareThreshold;

        public int highCareThreshold;

        [Space]
        public int defaultCareChange;
        public int onStationCareChange;
        
        [HideInInspector]
        public int careChange;
        [Space]
        public bool isTakingCareOf;

        public CareStation careStation;
        public bool HasOwnCareStation => careStation != null;
        public bool LowCare => currentCare < lowCareThreshold;
        public bool HighCare => currentCare >= highCareThreshold;
        public float Percentage => (float)currentCare / maxCare;
    }
}