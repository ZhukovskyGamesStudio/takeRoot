using System;
using Unity.Netcode;

public class NetworkDataHolder : NetworkBehaviour {
    public static NetworkDataHolder Instance;

    public static Action OnCreated;
    public static bool IsCreated = false;

    public SelectRaceNetworkData SelectRaceData ;
    public MainGameNetworkData MainGameNetworkData ;
    

    private void Awake() {
        Instance = this;
        IsCreated = true;
        OnCreated?.Invoke();
    }

    public void ResetSelections() {
        MainGameNetworkData.HostRace.Value = Race.None;
        MainGameNetworkData.ClientRace.Value = Race.None;
        SelectRaceData.HostReady.Value = false;
        SelectRaceData.ClientReady.Value = false;
    }
    
    public bool IsHost => NetworkManager.Singleton.IsHost;

    [ServerRpc(RequireOwnership = false)]
    public void ChooseClientRaceServerRpc(int client, Race race) {
        (client == 1 ? MainGameNetworkData.HostRace : MainGameNetworkData.ClientRace).Value = race;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientReadyServerRpc(int client) {
        (client == 1 ? SelectRaceData.HostReady : SelectRaceData.ClientReady).Value = true;
    }

    public static Race GetRace() {
        return NetworkManager.Singleton.IsHost ? Instance.MainGameNetworkData.HostRace.Value : Instance.MainGameNetworkData.ClientRace.Value;
    }
}