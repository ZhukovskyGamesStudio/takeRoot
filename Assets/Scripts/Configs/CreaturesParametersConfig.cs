using UnityEngine;

[CreateAssetMenu(fileName = "CreaturesParametersConfig", menuName = "Scriptable Objects/CreaturesParametersConfig")]
public class CreaturesParametersConfig : ScriptableObject {
    [field: SerializeField]
    public float PerformingTime { get; private set; }

    [field: SerializeField]
    public float AttackTime { get; private set; }

    [field: SerializeField]
    [Range(0, 1f)]
    public float AttackAnimationShift { get; private set; }

    [field: SerializeField]
    public float MoveTime { get; private set; }

    [field: SerializeField]
    [Min(1)]
    public int ViewRadius { get; private set; }
}