using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Color = UnityEngine.Color;

public class Gridable : ECSComponent {
    [SerializeField]
    private Vector2Int _size = Vector2Int.one; // Width and height in grid cells

    [field: SerializeField]
    public bool IsBlockingPath { get; private set; } = true;

    public Vector2Int GetBottomLeftOnGrid => VectorUtils.ToVector2Int(transform.position);
    public Vector3 GetCenterOnGrid => transform.position + new Vector3(_size.x / 2f, _size.y / 2f, 0) - Vector3.one / 2;

    public List<Vector2Int> GetOccupiedPositions() {
        Vector2Int pos = GetBottomLeftOnGrid;
        List<Vector2Int> r = new();
        for (int i = 0; i < _size.x; i++) {
            for (int j = 0; j < _size.y; j++) {
                r.Add(new Vector2Int(pos.x + i, pos.y + j));
            }
        }

        return r;
    }

    public List<Vector3> GetGridEdgePoints() {
        return new List<Vector3>() {
            transform.position - Vector3.one / 2,
            transform.position - Vector3.one / 2 + new Vector3(0, _size.y),
            transform.position - Vector3.one / 2 + new Vector3(_size.x, _size.y),
            transform.position - Vector3.one / 2 + new Vector3(_size.x, 0),
        };
    }

    private void OnDrawGizmos() {
        // Set Gizmo color to yellow
        Gizmos.color = Color.yellow;
        // Draw a wireframe box representing the entire occupied area
        Gizmos.DrawWireCube(GetCenterOnGrid, new Vector3(_size.x, _size.y, 0));
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) { }
}

public static class VectorUtils {
    public static Vector2Int ToVector2Int(Vector3 vector) {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
}