using Unity.Netcode;

public class MainGameNetworkData : NetworkData {
    public NetworkVariable<Race> HostRace;
    public NetworkVariable<Race> ClientRace;
}
