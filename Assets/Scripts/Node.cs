using UnityEngine;

public class Node {
    public readonly short PosX, PosY; // The grid position (X, Y)
    public bool Walkable; // Whether the node is walkable or not
    public short GCost; // Cost from the start node
    public short HCost; // Heuristic cost to the end node
    public short FCost => (short)(GCost + HCost); // Total cost (f = g + h)
    public Node Parent; // The parent node for path reconstruction

    public Node(Vector2Int pos, bool isWalkable) {
        PosX = (short)pos.x;
        PosY = (short)pos.y;
        Walkable = isWalkable;
    }
}