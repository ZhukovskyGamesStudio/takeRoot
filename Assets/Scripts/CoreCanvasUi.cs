using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CoreCanvasUi : NetworkBehaviour, IInitableInstance {
    [SerializeField]
    private InfoPanelView _infoPanel;

    [SerializeField]
    private SettlerInfoPanel _settlerPanel;

    [SerializeField]
    private Toggle _infoToggle;

    [field: SerializeField]
    public AvatarsView AvatarsView { get; private set; }

    [field: SerializeField]
    public ResearchPanelView ResearchPanelView { get; private set; }

    [field: SerializeField]
    public PanelsView PanelsView { get; private set; }

    [field: SerializeField]
    public PanelTogglesView PanelTogglesView { get; private set; }

    [field: SerializeField]
    public ResourcesView ResourcesView { get; private set; }

    public void Init() {
        ObsoleteCoreEntryPoint.UI = this;
        //TODO refactor
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

    public void OpenInfoPanel(ISelectable selectable) {
        CloseInfoPanel();
        _infoToggle.isOn = true;

        if (selectable.GetGameObject().TryGetComponent(out CraftingStationable craftingStationable)) {
            _infoPanel.Init(craftingStationable);
            return;
        }

        _infoPanel.Init(selectable.GetInfoData());
    }

    public void CloseInfoPanel() {
        _settlerPanel.gameObject.SetActive(false);
        _infoPanel.gameObject.SetActive(false);
        _infoToggle.isOn = false;
    }
}