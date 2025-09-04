using Unity.Netcode;

public class SelectRaceNetworkData : NetworkData {
    public NetworkVariable<bool> HostReady;
    public NetworkVariable<bool> ClientReady;
}
