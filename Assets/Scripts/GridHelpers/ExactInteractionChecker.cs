using System.Linq;
using UnityEngine;

public static class ExactInteractionChecker {
    public static bool CanInteract(Vector2Int from, Interactable to) {
        return from.x == to.GetInteractableSell.x && from.y == to.GetInteractableSell.y;
    }

    public static Vector2Int? NextStepOnPath(Vector2Int from, Vector2Int to) {
        var path = AStarPathfinding.Instance.FindPath(from, to);
        if (path.Count > 0) {
            return path.First();
        }

        return null;
    }
}