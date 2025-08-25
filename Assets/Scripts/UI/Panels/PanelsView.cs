using UnityEngine;

public class PanelsView : MonoBehaviour {
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<PanelType, GameObject> Panels { get; private set; } = new();
}

