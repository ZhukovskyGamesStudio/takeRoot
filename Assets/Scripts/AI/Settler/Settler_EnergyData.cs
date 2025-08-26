using System;

namespace AI {
    [Serializable]
    public class Settler_EnergyData {
        public int maxEnergy;
        public int currentEnergy;
        public int tiredThreshold;
        public int criticalTiredThreshold;
        public int energyChange;
        public int defaultEnergyChange;
        public int onGroundEnergyChange;
        public float energyCooldown;
        public float energyTimer;
        public bool isSleeping;
        public Bed bed;
        public bool HasOwnBed => bed != null;
        public bool IsTired => currentEnergy < tiredThreshold;
        public bool IsCriticalTired => currentEnergy < criticalTiredThreshold;
    }
}