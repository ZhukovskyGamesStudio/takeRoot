using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldObjects;

public static class ExactInteractionChecker {
    public static bool CanInteract(Vector2Int from, Interactable to) {
        return from.x == to.GetInteractableCell.x && from.y == to.GetInteractableCell.y;
    }
    
    public static bool CanInteract(Vector2Int from, TacticalInteractable to) {
        return from.x == to.GetInteractableCell.x && from.y == to.GetInteractableCell.y;
    }

    public static bool CanInteractFromNeighborCell(Vector2Int from, Interactable to)
    {
        return to.InteractableCells.Contains(from);
    }

    public static bool CanInteractFromNeighborCell(Vector2Int from, TacticalInteractable to)
    {
        return to.InteractableCells.Contains(from); 
    }


    public static Vector2Int? NextStepOnPath(Vector2Int from, HashSet<Vector2Int> to) {
        var path = AStarPathfinding.Instance.FindPath(from, to);
        if (path.Count > 0) {
            return path.First();
        }

        return null;    
    }

    public static Vector2Int? NextStepOnPathForZombies(Vector2Int from, HashSet<Vector2Int> to, int offsetX)
    {
        var path = AStarPathfinding.Instance.FindPathForZombies(from, to, offsetX);
        if (path.Count > 0)
        {
            return path.First();
        }

        return null;
    }    
    public static Vector2Int? NextStepForZombieOnPathWithWallsAsObstacle(Vector2Int from, HashSet<Vector2Int> to, int offsetX)
    {
        var path = AStarPathfinding.Instance.FindPathForZombiesWithWallsAsObstacle(from, to, offsetX);
        if (path.Count > 0)
        {
            return path.First();
        }

        return null;
    }
}