using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettlersSelectionManager : MonoBehaviour {
    public static SettlersSelectionManager Instance;

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
        Instance = this;
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
    }

    private void CreateSelectionView() {
        _selectionView = Instantiate(_selectionViewPrefab, transform);
        _selectionView.gameObject.SetActive(false);
    }

    private void SelectSettler(Settler settler) {
        if (SelectedSettler != null) {
            return;
        }

        if (settler.Race != Core.Instance.MyRace() && settler.Race != Race.Both) {
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
    }

    private void ChangePanels(bool isTactical) {
        CommandsManagersHolder.Instance.TacticalCommandsManager.SetActivePanel(isTactical);
        CommandsManagersHolder.Instance.CommandsManager.SetActivePanel(!isTactical);
    }

    public void TryChangeSelectedSettlerMode(bool isTactical) {
        if (SelectedSettler == null) {
            return;
        }

        SelectedSettler.ChangeMode(isTactical ? Mode.Tactical : Mode.Planning);
    }
}