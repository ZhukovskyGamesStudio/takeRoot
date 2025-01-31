using Unity.Netcode;
using UnityEngine;

public class CoreCanvasUi : NetworkBehaviour, IInitableInstance {
    [field: SerializeField]
    public NetworkReplacementUi NetworkReplacement { get; private set; }

    public void Init() {
        Core.UI = this;
        InitRace();

        if (NetworkManager.Singleton == null) {
            NetworkReplacement.gameObject.SetActive(true);
            NetworkReplacement.OnChangeRace += SetRace;
        }
    }

    private void InitRace() {
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