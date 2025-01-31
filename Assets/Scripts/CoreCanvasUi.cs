using Unity.Netcode;
using UnityEngine;

public class CoreCanvasUi : NetworkBehaviour {
    [field: SerializeField]
    public NetworkReplacementUi NetworkReplacement { get; private set; }

    private void Awake() {
        if (NetworkManager.Singleton == null) {
            NetworkReplacement.gameObject.SetActive(true);
            NetworkReplacement.OnChangeRace += SetRace;
        }
    }

    public void InitRace() {
        if (NetworkManager.Singleton != null) {
            SetRace(PlayerRaceSelection.GetRace());
        } else {
            SetRace(NetworkReplacement.CurrentRace);
        }
    }

    private void SetRace(Race race) {
        IHasRaceVariant[] variableChildren = transform.GetComponentsInChildren<IHasRaceVariant>();
        foreach (IHasRaceVariant variable in variableChildren) {
            variable.SetVariant(race);
        }
    }
}