using UniRx;
using UnityEngine;

public class NetworkService : INetworkService {
    public ReactiveProperty<Race> MyRace { get; set; } = new ReactiveProperty<Race>();

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