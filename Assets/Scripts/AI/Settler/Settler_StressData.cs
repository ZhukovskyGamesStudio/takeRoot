using System;
using Unity.Netcode;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_StressData : INetworkSerializable{
        public int maxStress;
        public float currentStress;
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
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref maxStress);
            serializer.SerializeValue(ref currentStress);
            serializer.SerializeValue(ref breakdownStressThreshold);
            serializer.SerializeValue(ref breakdownChance);
            serializer.SerializeValue(ref breakdownDuration);
            serializer.SerializeValue(ref breakdownTimer);
            serializer.SerializeValue(ref stressAfterBreakdown);
            serializer.SerializeValue(ref stressChange);
            serializer.SerializeValue(ref lowSatietyStressChange);
            serializer.SerializeValue(ref highSatietyStressChange);
        }
    }
}