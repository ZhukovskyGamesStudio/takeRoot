using UnityEngine;
using UnityEngine.UI;

public class PanelTogglesView : MonoBehaviour {
    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<PanelType, Toggle> _toggles;
}