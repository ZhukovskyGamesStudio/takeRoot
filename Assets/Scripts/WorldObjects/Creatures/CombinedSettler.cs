using System.Linq;
using UnityEngine;

public class CombinedSettler : Settler
{
    protected override void Awake()
    {
        base.Awake();
        GetEcsComponent<Damagable>().OnDiedAction += OnDied;
    }

    public void OnDied()
    {
        Core.SettlersManager.DestroySettler(this);
        SpawnSettlersAround();
    }

    private void SpawnSettlersAround()
    {
        Vector2Int spawnPos = new Vector2Int(999, 999);
        Race race = Race.Plants;
        int remainingSpawnPos = 2;
        while (remainingSpawnPos > 0)
        {
            var pos = _gridable.InteractableCells.ElementAt(Random.Range(0, _gridable.InteractableCells.Count));
            if (spawnPos != pos && AStarPathfinding.IsWalkable(pos))
            {
                spawnPos = pos;
                Core.SettlersManager.SpawnSettlerAt(race, spawnPos);
                remainingSpawnPos--;
                race = Race.Robots;
            }
        }

    }
}