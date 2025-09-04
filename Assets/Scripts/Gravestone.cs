using AI;
using Unity.Netcode;
using UnityEngine;

public class Gravestone : NetworkBehaviour {
    public void SetData(string settlerName, DeathCause cause) {
        UpdateMainData(settlerName, cause.ToString());
        UpdateGravestoneClientRpc(settlerName, cause.ToString());
    }

    [ClientRpc]
    private void UpdateGravestoneClientRpc(string settlerName, string cause) {
        UpdateMainData(settlerName, cause);
    }

    private void UpdateMainData(string settlerName, string cause) {
        GetComponent<CommandTarget>().Data.MainInfoData.Name = settlerName;
        GetComponent<CommandTarget>().Data.MainInfoData.Description = cause;
    }
}