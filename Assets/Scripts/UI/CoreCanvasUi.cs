using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CoreCanvasUi : NetworkBehaviour {
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
    public FarmingPanelView FarmingPanelView { get; private set; }

    [field: SerializeField]
    public PanelsView PanelsView { get; private set; }

    [field: SerializeField]
    public PanelTogglesView PanelTogglesView { get; private set; }

    [field: SerializeField]
    public ResourcesView ResourcesView { get; private set; }
    
    [field: SerializeField]
    public OverlaysView OverlaysView { get; private set; }
    [field: SerializeField]
    public NotificationsView NotificationsView { get; private set; }

    [SerializeField]
    private RectTransform _selection;

    public void InitRace(Race race) {
        SetRace(race);
    }

    private void SetRace(Race race) {
        IHasRaceVariant[] variableChildren = transform.GetComponentsInChildren<IHasRaceVariant>();
        foreach (IHasRaceVariant variable in variableChildren) {
            variable.SetVariant(race);
        }
    }

    private void Update() {
        UpdateSelection();
    }

    private void UpdateSelection() {
        if (Input.GetMouseButtonDown(0)) {
            _selection.gameObject.SetActive(true);
            _selection.localPosition = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
            UpdateSelectionSize();
        }
        else if (Input.GetMouseButton(0)) {
            UpdateSelectionSize();
        } else {
            _selection.gameObject.SetActive(false);
        }
    }
    
    private void UpdateSelectionSize() 
    {
        Vector2 sizeDelta = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f) - _selection.localPosition;
        _selection.pivot = new Vector2(sizeDelta.x >= 0 ? 0 : 1, sizeDelta.y >= 0 ? 0 : 1);
        _selection.sizeDelta = new Vector2(Mathf.Abs(sizeDelta.x), Mathf.Abs(sizeDelta.y));
    }

    [Obsolete]
    public void OpenInfoPanel(ISelectable selectable) {
       /*
        CloseInfoPanel();
        _infoToggle.isOn = true;

        if (selectable.GetGameObject().TryGetComponent(out CraftingStationable craftingStationable)) {
            _infoPanel.Init(craftingStationable);
            return;
        }

        _infoPanel.Init(selectable.GetInfoData());*/
    }

    public void CloseInfoPanel() {
        _settlerPanel.gameObject.SetActive(false);
        _infoPanel.gameObject.SetActive(false);
        _infoToggle.isOn = false;
    }

    public void ExitCore() {
        NetworkManager.Singleton.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}