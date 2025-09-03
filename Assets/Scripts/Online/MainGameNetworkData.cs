using Unity.Netcode;

public class MainGameNetworkData : NetworkData {
    public NetworkVariable<Race> HostRace = new(Race.None);
    public NetworkVariable<Race> ClientRace = new(Race.None);
}
