using System.Collections.Generic;
using UnityEngine;

public static class LineOfViewAlgorithm {
    public static bool CanSee(Vector2Int from, Vector2Int to, HashSet<Vector2Int> blockingViews) {
        foreach (Vector2Int tile in GetLineTiles(from, to)) {
            if (blockingViews.Contains(tile)) {
                return false;
            }
        }

        return true;
    }

    //🔥 Solution: Bresenham's Line Algorithm
    // The Bresenham line algorithm efficiently finds all grid tiles between two points, making it perfect for checking if any tile blocks the view.
    private static List<Vector2Int> GetLineTiles(Vector2Int from, Vector2Int to) {
        List<Vector2Int> lineTiles = new();

        int dx = Mathf.Abs(to.x - from.x);
        int dy = Mathf.Abs(to.y - from.y);

        int sx = from.x < to.x ? 1 : -1;
        int sy = from.y < to.y ? 1 : -1;

        int err = dx - dy;

        Vector2Int current = from;
        while (current != to) {
            lineTiles.Add(current);

            int e2 = 2 * err;
            if (e2 > -dy) {
                err -= dy;
                current.x += sx;
            }

            if (e2 < dx) {
                err += dx;
                current.y += sy;
            }
        }

        return lineTiles;
    }
}