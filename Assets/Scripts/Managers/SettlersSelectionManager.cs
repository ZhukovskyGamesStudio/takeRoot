using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettlersSelectionManager : MonoBehaviour {

    [SerializeField]
    private CommandsPanel _commandsPanel;

    [SerializeField]
    private TacticalCommandPanel _tacticalCommandPanel;

    [SerializeField]
    private ChangeModeToggle _changeModeToggle;

    [SerializeField]
    private SelectionView _selectionViewPrefab;

    private SelectionView _selectionView;

    public Settler SelectedSettler { get; private set; }

    private void Awake() {
        Core.SettlersSelectionManager = this;
        CreateSelectionView();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (EventSystem.current?.IsPointerOverGameObject() == true) return;
            if (!hit) {
                TryUnselectSettler();
                return;
            }

            if (hit.transform.TryGetComponent(out Settler settler)) {
                TryUnselectSettler();
                SelectSettler(settler);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                TryUnselectSettler();
            }
        }

        TryAutoOpenInfoPanel();
    }

    public void TryUnselectSpecificSettler(Settler settler)
    {
        _tacticalCommandPanel.ClearSelectedTacticalCommand(_tacticalCommandPanel.SelectedTacticalCommand);
        if (SelectedSettler == settler) {
            TryUnselectSettler();
        }
    }

    private void TryAutoOpenInfoPanel() {
        if (!Input.GetMouseButton(0) || _commandsPanel.SelectedCommand != Command.None || SelectedSettler == null) {
            return;
        }

        if (Core.UI.InfoPanelView.IsAutoOpenInfoPanel) {
            Core.UI.InfoPanelView.SetToggle(true);
        }

        Core.UI.InfoPanelView.Init(SelectedSettler.SettlerData);
    }

    private void CreateSelectionView() {
        _selectionView = Instantiate(_selectionViewPrefab, transform);
        _selectionView.gameObject.SetActive(false);
    }

    private void SelectSettler(Settler settler) {
        if (SelectedSettler != null) {
            return;
        }

        if (settler.SettlerData.Race != Core.Instance.MyRace() && settler.SettlerData.Race != Race.Both) {
            return;
        }

        SelectedSettler = settler;
        _changeModeToggle.gameObject.SetActive(true);
        _changeModeToggle.SetToggleValue(SelectedSettler.Mode == Mode.Tactical);
        Gridable gridable = SelectedSettler.GetEcsComponent<Gridable>();
        _selectionView.Init(gridable, gridable.transform);
        ChangePanels(SelectedSettler.Mode == Mode.Tactical);
    }

    private void TryUnselectSettler() {
        if (SelectedSettler == null || _tacticalCommandPanel.SelectedTacticalCommand != TacticalCommand.None) {
            return;
        }

        ChangePanels(false);
        _changeModeToggle.gameObject.SetActive(false);
        _changeModeToggle.SetToggleValue(false);
        //SelectedSettler.ChangeMode(Mode.Planning);
        SelectedSettler = null;
        _selectionView.Release(transform);

        if (Core.UI.InfoPanelView.GetToggle()) {
            Core.UI.InfoPanelView.SetToggle(false);
        }
    }

    private void ChangePanels(bool isTactical) {
        Core.CommandsManagersHolder.TacticalCommandsManager.SetActivePanel(isTactical);
        Core.CommandsManagersHolder.CommandsManager.SetActivePanel(!isTactical);
    }

    public void TryChangeSelectedSettlerMode(bool isTactical) {
        if (SelectedSettler == null) {
            return;
        }

        SelectedSettler.ChangeMode(isTactical ? Mode.Tactical : Mode.Planning);
        Core.GameEventsManager.WorldObjectsEvents.OnSettlerModeChanged(SelectedSettler);
    }
}