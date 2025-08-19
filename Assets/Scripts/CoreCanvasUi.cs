using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CoreCanvasUi : NetworkBehaviour, IInitableInstance {
    [field: SerializeField]
    public NetworkReplacementUi NetworkReplacement { get; private set; }

    [field: SerializeField]
    public InfoBookView InfoPanelView;

    public void Init() {
        Core.UI = this;
        InitRace();
        InitRaceChangeButton();
    }

    private void InitRaceChangeButton() {
        if (NetworkManager.Singleton == null) {
            NetworkReplacement.gameObject.SetActive(true);
            NetworkReplacement.OnChangeRace += SetRace;
        }
    }

    private void InitRace() {
        if (NetworkManager.Singleton != null) {
            SetRace(PlayerRaceSelection.GetRace());
        } else {
            SetRace(Core.Instance.CurrentNetworkFakeRace);
        }
    }

    private void SetRace(Race race) {
        IHasRaceVariant[] variableChildren = transform.GetComponentsInChildren<IHasRaceVariant>();
        foreach (IHasRaceVariant variable in variableChildren) {
            variable.SetVariant(race);
        }
    }
}