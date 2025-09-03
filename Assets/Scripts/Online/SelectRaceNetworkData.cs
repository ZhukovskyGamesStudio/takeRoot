using Unity.Netcode;

public class SelectRaceNetworkData : NetworkData {
    public NetworkVariable<bool> HostReady = new(false);
    public NetworkVariable<bool> ClientReady = new(false);
}
