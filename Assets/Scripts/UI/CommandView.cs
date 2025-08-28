using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandView : MonoBehaviour {
    public Sprite onButton;
    public Sprite offButton;

    public TextMeshProUGUI CurrentCommand;

    public Button DestroyCommandButton;
    public Button SearchCommandButton;
    public Button WaterCommandButton;
    public Button CancelCommandButton;
    public Button MoveCommandButton;
    
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<JobType, Toggle> Toggles { get; private set; } = new();
}