using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExactInteractionChecker {
    public static bool CanInteract(Vector2Int from, Interactable to) {
        return from.x == to.GetInteractableSell.x && from.y == to.GetInteractableSell.y;
    }

    public static Vector2Int NextStepOnPath(Vector2Int from, Interactable to) {
        return  AStarPathfinding.Instance.FindPath(from, to.GetInteractableSell).First();
        return CalculatePath(from, to.GetInteractableSell).First();
    }

    private static List<Vector2Int> CalculatePath(Vector2Int from, Vector2Int to) {
        List<Vector2Int> path = new List<Vector2Int>();
        for (int i = 0; i < 10000; i++) {
            if (from == to) {
                return path;
            }

            Vector2Int direction = to - from;
            if (direction.x != 0) {
                direction.x = Mathf.RoundToInt(Mathf.Sign(direction.x));
            }

            if (direction.y != 0) {
                direction.y = Mathf.RoundToInt(Mathf.Sign(direction.y));
            }

            path.Add(from + direction);
            from += direction;
        }

        return path;
    }
}