using UnityEngine;
using UnityEngine.Serialization;

public class ZombieData : ECSComponent
{
    public EnemyState State = EnemyState.Idle;
    
    public int PointsToRageState;
    public int CurrentRagePoints;
    
    public Vector2Int GetCellOnGrid => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    
    
    
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    { }
}