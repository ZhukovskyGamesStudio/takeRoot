using UniRx;
using Unity.Netcode;
using UnityEngine;

public class NetworkService : INetworkService {
    public bool IsHost => AdminManager.IsFakeOnline || NetworkManager.Singleton.IsHost;
    public ReactiveProperty<Race> MyRace { get; set; } = new ReactiveProperty<Race>();

    public T InstantiateAndSpawn<T>(T prefab, Vector3 spawnPos = default, Quaternion rot = default) where T : NetworkBehaviour {
        var res = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(prefab.GetComponent<NetworkObject>(), position: spawnPos,
            rotation: rot);
        return res.GetComponent<T>();
    }

    public NetworkDataHolder NetworkDataHolder => NetworkDataHolder.Instance;

    public NetworkService(NetworkDataHolder networkDataHolderPrefab) {
        if (NetworkDataHolder.Instance == null) {
            NetworkDataHolder.Instance = Object.Instantiate(networkDataHolderPrefab);
        }

        MyRace.Value = (NetworkDataHolder.IsHost
            ? NetworkDataHolder.MainGameNetworkData.HostRace
            : NetworkDataHolder.MainGameNetworkData.ClientRace).Value;

        if (AdminManager.IsFakeOnline) {
            MyRace.Value = Race.Plants;
        }
    }
}