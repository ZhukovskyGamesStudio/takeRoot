using UniRx;
using Unity.Netcode;
using UnityEngine;

public interface INetworkService : IService {
    
    public NetworkDataHolder NetworkDataHolder  { get; }
     
    public bool IsHost { get; }
    
    public ReactiveProperty<Race> MyRace { get; set; }

    public T InstantiateAndSpawn<T>(T prefab, Vector3 spawnPos = default, Quaternion rot = default) where T : NetworkBehaviour;
}