using UnityEngine;

[CreateAssetMenu(fileName = "IngameTimeConfig", menuName = "Scriptable Objecst/IngameTimeConfig", order = 0)]
public class IngameTimeConfig : ScriptableObject {
    [field: SerializeField]
    [Min(0)]
    public float IngameDayInMinutes { get; private set; } = 12f;

    [field: SerializeField]
    [Range(0, 1f)]
    public float DaytimeStartPercent;

    [field: SerializeField]
    [Range(0, 1f)]
    public float DaytimePercent;
    
    
    public Gradient DaynightLightColorGradient;
}