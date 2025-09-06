using Unity.Netcode;

public class ResearchNetworkData : NetworkData {
    public NetworkVariable<Research> CurrentResearch = new();

    public void SelectResearch(Research research) {
        SelectResearchServerRpc(research);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectResearchServerRpc(Research research) {
        CurrentResearch.Value = research;
    }
}