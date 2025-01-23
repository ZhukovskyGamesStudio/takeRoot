using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettlersSelectionManager : MonoBehaviour
{
    public static SettlersSelectionManager Instance;
        
    [SerializeField]private CommandsPanel _commandsPanel;
    [SerializeField]private TacticalCommandPanel _tacticalCommandPanel;
    [SerializeField]private ChangeModeToggle _changeModeToggle;

    public Settler SelectedSettler { get; private set; }
        
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (EventSystem.current?.IsPointerOverGameObject() == true) return;
            if (!hit) 
            {
                TryUnselectSettler();
                return;
            }
            if (hit.transform.TryGetComponent(out Settler settler))
            {
                TryUnselectSettler();
                SelectSettler(settler);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TryUnselectSettler();
            }
        }
    }

    private void SelectSettler(Settler settler)
    {
        if (SelectedSettler == null)
        {
            SelectedSettler = settler;
            _changeModeToggle.gameObject.SetActive(true);
        }
    }

    private void TryUnselectSettler()
    {
        if (SelectedSettler != null && _tacticalCommandPanel.SelectedTacticalCommand == TacticalCommand.None)
        {
            _changeModeToggle.gameObject.SetActive(false);
            _changeModeToggle.GetComponent<Toggle>().isOn = false;
            SelectedSettler.ChangeMode(Mode.Planning);
            SelectedSettler = null;
        }
    }
}