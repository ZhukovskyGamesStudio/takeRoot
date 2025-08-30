using UnityEngine;

[CreateAssetMenu(fileName = "TimeScaleConfig", menuName = "Scriptable Objects/TimeScaleConfig")]
public class TimeScaleConfig : ScriptableObject {
    public AYellowpaper.SerializedCollections.SerializedDictionary<GameSpeedType, float> TimeScales;
}
