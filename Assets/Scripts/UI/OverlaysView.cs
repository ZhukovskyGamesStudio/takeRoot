using UnityEngine;
using UnityEngine.UI;

public class OverlaysView : MonoBehaviour
{
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<OverlayType, Toggle> Toggles { get; private set; } = new();
}
