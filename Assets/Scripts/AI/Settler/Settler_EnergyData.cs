using System;
using Unity.Netcode;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_EnergyData : INetworkSerializable {
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

        public bool IsTired => currentEnergy < tiredThreshold;
        public bool IsCriticalTired => currentEnergy < criticalTiredThreshold;
        public float Percentage => (float)currentEnergy / maxEnergy;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref maxEnergy);
            serializer.SerializeValue(ref currentEnergy);
            serializer.SerializeValue(ref tiredThreshold);
            serializer.SerializeValue(ref criticalTiredThreshold);
            serializer.SerializeValue(ref energyChange);
            serializer.SerializeValue(ref defaultEnergyChange);
            serializer.SerializeValue(ref onGroundEnergyChange);
            serializer.SerializeValue(ref onBedEnergyChange);
            serializer.SerializeValue(ref isSleeping);
        }
    }
}