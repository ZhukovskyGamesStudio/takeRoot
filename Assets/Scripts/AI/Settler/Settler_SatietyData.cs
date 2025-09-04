using System;
using Unity.Netcode;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_SatietyData : INetworkSerializable{
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
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref maxSatiety);
            serializer.SerializeValue(ref currentSatiety);
            serializer.SerializeValue(ref lowSatietyThreshold);
            serializer.SerializeValue(ref highSatietyThreshold);
            serializer.SerializeValue(ref satietyChange);
            serializer.SerializeValue(ref isDrinking);
        }
    }
}