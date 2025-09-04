using UnityEngine;
using UnityEngine.UI;

public class CommandView : MonoBehaviour {
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<JobType, Toggle> Toggles { get; private set; } = new();
}