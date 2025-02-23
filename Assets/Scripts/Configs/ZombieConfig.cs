using UnityEngine;

[CreateAssetMenu(fileName = "ZombieConfig", menuName = "Scriptable Objects/ZombieConfig")]
public class ZombieConfig : ScriptableObject {
    [field: SerializeField]
    public float MoveTime { get; private set; }

    [field: SerializeField]
    public float MovePause { get; private set; } = 1;

    [field: SerializeField]
    public float MoveCooldown { get; private set; }

    [field: SerializeField]
    public float ChangeTargetCooldown { get; private set; }

    [field: SerializeField]
    public float DestroyTime { get; private set; }

    [field: SerializeField]
    public float AttackTime { get; private set; }

    [field: SerializeField]
    public int DestroyDamage { get; private set; }
}