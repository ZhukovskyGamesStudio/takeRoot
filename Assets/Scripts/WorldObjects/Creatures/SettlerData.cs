using UnityEngine;

public class SettlerData : ECSComponent {
    [field: SerializeField]
    public Mood _mood;

    [field: SerializeField]
    public Mode _mode;

    public Race Race;

    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) { }
}