using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_EnergyData {
        public int maxEnergy;
        [HideInInspector]
        public float currentEnergy;
        [Space]
        public int tiredThreshold;
        public int criticalTiredThreshold;
      
        [HideInInspector]
        public float energyChange;
        [Space]
        public float defaultEnergyChange;
        public float onGroundEnergyChange;
        public float onBedEnergyChange;
        [Space]
        public bool isSleeping;
        public Bed bed;
        public bool HasOwnBed => bed != null;
        public bool IsTired => currentEnergy < tiredThreshold;
        public bool IsCriticalTired => currentEnergy < criticalTiredThreshold;
        public float Percentage => (float)currentEnergy / maxEnergy;
    }
}