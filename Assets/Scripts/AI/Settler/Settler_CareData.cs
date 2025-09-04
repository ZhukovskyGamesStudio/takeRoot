using System;
using Unity.Netcode;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_CareData: INetworkSerializable {
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
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref maxCare);
            serializer.SerializeValue(ref currentCare);
            serializer.SerializeValue(ref lowCareThreshold);
            serializer.SerializeValue(ref highCareThreshold);
            serializer.SerializeValue(ref defaultCareChange);
            serializer.SerializeValue(ref onStationCareChange);
            serializer.SerializeValue(ref careChange);
            serializer.SerializeValue(ref isTakingCareOf);
        }
    }
}