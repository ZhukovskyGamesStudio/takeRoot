using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_EnergyData {
        public int maxEnergy;
        public int currentEnergy;
        [Space]
        public int tiredThreshold;
        public int criticalTiredThreshold;
      
        [HideInInspector]
        public int energyChange;
        [Space]
        public int defaultEnergyChange;
        public int onGroundEnergyChange;
        public int onBedEnergyChange;
        [Space]
        public bool isSleeping;
        public Bed bed;
        public bool HasOwnBed => bed != null;
        public bool IsTired => currentEnergy < tiredThreshold;
        public bool IsCriticalTired => currentEnergy < criticalTiredThreshold;
        public float Percentage => (float)currentEnergy / maxEnergy;
    }
}