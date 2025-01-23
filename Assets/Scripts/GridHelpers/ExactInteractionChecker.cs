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

    public static Vector2Int? NextStepOnPath(Vector2Int from, Vector2Int to) {
        var path = AStarPathfinding.Instance.FindPath(from, to);
        if (path.Count > 0) {
            return path.First();
        }

        return null;
    }
}