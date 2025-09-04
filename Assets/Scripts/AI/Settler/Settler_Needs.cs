using System;
using AI;
using Unity.Netcode;

[Serializable]
public class Settler_Needs : INetworkSerializable {
    public float Hp;
    public int MaxHp;

    public Settler_EnergyData Energy;
    public Settler_StressData StressData;
    public Settler_SatietyData SatietyData;
    public Settler_CareData CareData;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        
        // При чтении гарантируем, что объекты созданы
        if (serializer.IsReader) {
            Energy ??= new Settler_EnergyData();
            StressData ??= new Settler_StressData();
            SatietyData ??= new Settler_SatietyData();
            CareData ??= new Settler_CareData();
        }
        
        
        serializer.SerializeValue(ref Hp);
        serializer.SerializeValue(ref MaxHp);
        serializer.SerializeValue(ref Energy);
        serializer.SerializeValue(ref StressData);
        serializer.SerializeValue(ref SatietyData);
        serializer.SerializeValue(ref CareData);
    }
}