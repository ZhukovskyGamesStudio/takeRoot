using System.Collections.Generic;
using UnityEngine;
using WorldObjects;


public class Noisable : ECSComponent
{
    [SerializeField]private int _noiseRadius;
    private Gridable _gridable;
        
    public void MakeNoise()
    {
        var noisePosition = _gridable.GetCenterOnGrid.ToVector2Int();
        var listeners = FindObjectsByType<Listener>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        
        HashSet<Vector2Int> noisePoints = new HashSet<Vector2Int>();
            
        var topLeft = new Vector2Int(noisePosition.x - _noiseRadius, noisePosition.y + _noiseRadius);
        var botRight = new Vector2Int(noisePosition.x + _noiseRadius, noisePosition.y - _noiseRadius);
            
        for (int x = topLeft.x; x < botRight.x; x++)
        for (int y = topLeft.y; y > botRight.y; y--)
        {
            noisePoints.Add(new Vector2Int(x, y));
        }
        foreach (Listener listener in listeners)
        {
            foreach (Vector2Int occupiedPosition in listener.Gridable.GetOccupiedPositions())
                if (noisePoints.Contains(occupiedPosition))
                    listener.HasHeard?.Invoke(noisePosition);
        }
    }
        
    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
        _gridable = entity.GetEcsComponent<Gridable>();
    }
}