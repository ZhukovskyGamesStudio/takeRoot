using UnityEngine;
using UnityEngine.UI;

public class PanelTogglesView : MonoBehaviour {
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<PanelType, Toggle> Toggles { get; private set; } = new();
}