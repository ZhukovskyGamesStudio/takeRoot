using System;
using Unity.Netcode;

public class PlayerRaceSelection : NetworkBehaviour {
    public static PlayerRaceSelection Instance;

    public static Action OnCreated;
    public static bool IsCreated = false;
    public NetworkVariable<Race> Player1Race = new NetworkVariable<Race>(Race.None);
    public NetworkVariable<Race> Player2Race = new NetworkVariable<Race>(Race.None);

    public NetworkVariable<bool> Player1Ready = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> Player2Ready = new NetworkVariable<bool>(false);

    private void Awake() {
        Instance = this;
        IsCreated = true;
        OnCreated?.Invoke();
    }

    public void ResetSelections() {
        Player1Race.Value = Race.None;
        Player2Race.Value = Race.None;
        Player1Ready.Value = false;
        Player2Ready.Value = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChooseClientRaceServerRpc(int client, Race race) {
        (client == 1 ? Player1Race : Player2Race).Value = race;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientReadyServerRpc(int client) {
        (client == 1 ? Player1Ready : Player2Ready).Value = true;
    }

    public static Race GetRace() {
        return NetworkManager.Singleton.IsHost ? Instance.Player1Race.Value : Instance.Player2Race.Value;
    }
}