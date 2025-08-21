using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CoreCanvasUi : NetworkBehaviour, IInitableInstance {
    [field: SerializeField]
    public NetworkReplacementUi NetworkReplacement { get; private set; }

    [SerializeField]
    private InfoBookView _infoPanel;

    [SerializeField]
    private SettlerPanel _settlerPanel;
    
    [SerializeField]
    private Toggle _infoToggle;

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

    public void OpenInfoPanel(ISelectable selectable) {
        CloseInfoPanel();
        _infoToggle.isOn = true;

        if (selectable.GetGameObject().TryGetComponent(out CraftingStationable craftingStationable)) {
            _infoPanel.Init(craftingStationable);
            return;
        }
        
        _infoPanel.Init(selectable.GetInfoData());
    }

    public void OpenSettlerPanel(SettlerData settlerData) {
        CloseInfoPanel();
        
        _settlerPanel.gameObject.SetActive(true);
        _settlerPanel.Init(settlerData);
    }

    public void CloseInfoPanel() {
        _settlerPanel.gameObject.SetActive(false);
        _infoPanel.gameObject.SetActive(false);
        _infoToggle.isOn = false;
    }
}