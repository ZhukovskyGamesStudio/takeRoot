using UnityEngine;

public class Node
{
    public Vector2Int position;  // The grid position (X, Y)
    public bool walkable;        // Whether the node is walkable or not
    public int gCost;            // Cost from the start node
    public int hCost;            // Heuristic cost to the end node
    public int fCost => gCost + hCost;  // Total cost (f = g + h)
    public Node parent;          // The parent node for path reconstruction

    public Node(Vector2Int pos, bool isWalkable)
    {
        position = pos;
        walkable = isWalkable;
    }
}