using Settlers.UI.Panels;
using UnityEngine;
using UnityEngine.UI;

public class PanelTogglesView : MonoBehaviour {
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<PanelType, Toggle> Toggles { get; private set; } = new();
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<PanelType, ToggleData> ToggleData { get; private set; } = new();
}