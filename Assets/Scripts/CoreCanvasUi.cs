using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CoreCanvasUi : NetworkBehaviour, IInitableInstance {
    [field: SerializeField]
    public InfoBookView InfoPanelView;

    public void Init() {
        ObsoleteCoreEntryPoint.UI = this;
        ObsoleteCoreEntryPoint.Instance.OnChangeRace += SetRace;
        InitRace();
    }

    private void InitRace() {
        if (NetworkManager.Singleton != null) {
            SetRace(PlayerRaceSelection.GetRace());
        } else {
            SetRace(ObsoleteCoreEntryPoint.Instance.CurrentNetworkFakeRace);
        }
    }

    private void SetRace(Race race) {
        IHasRaceVariant[] variableChildren = transform.GetComponentsInChildren<IHasRaceVariant>();
        foreach (IHasRaceVariant variable in variableChildren) {
            variable.SetVariant(race);
        }
    }

    public override void OnDestroy() {
        ObsoleteCoreEntryPoint.Instance.OnChangeRace -= SetRace;
        base.OnDestroy();
    }
}