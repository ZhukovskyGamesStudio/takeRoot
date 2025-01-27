using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

public class Gridable : ECSComponent {
    [SerializeField]
    private Vector2Int _size = Vector2Int.one; // Width and height in grid cells

    [field: SerializeField]
    public bool IsBlockingPath { get; private set; } = true;
    
    public Vector2Int GetBottomLeftOnGrid { get => VectorUtils.ToVector2Int(transform.position);
        private set{}
    }

    public Vector3 GetCenterOnGrid { get => transform.position + new Vector3(_size.x / 2f, _size.y / 2f, 0) - Vector3.one / 2;
        private set{}
    }

    public HashSet<Vector2Int> InteractableCells = new HashSet<Vector2Int>();

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
        //DrawInteractableCells();
        
        // Set Gizmo color to yellow
        Gizmos.color = Color.yellow;
        // Draw a wireframe box representing the entire occupied area
        Gizmos.DrawWireCube(GetCenterOnGrid, new Vector3(_size.x, _size.y, 0));

        DrawInteractableCells();
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity)
    { }

    private void Start()
    {
        GetBottomLeftOnGrid = VectorUtils.ToVector2Int(transform.position);
        GetCenterOnGrid = transform.position + new Vector3(_size.x / 2f, _size.y / 2f, 0) - Vector3.one / 2;
        GetNeighbors(GetOccupiedPositions());
    }

    private void DrawInteractableCells()
    {
        Gizmos.color = Color.green;
        Vector3 centerShift = new Vector3(1 / 2f, 1 / 2f, 0) - Vector3.one / 2; 
        foreach (Vector2Int cell in InteractableCells)
        {
            Gizmos.DrawWireCube(new Vector3(cell.x, cell.y) + centerShift, new Vector3(1, 1, 0));
        }
    }

    private void GetNeighbors(List<Vector2Int> cells)
    {
        HashSet<Vector2Int> neighbors = new HashSet<Vector2Int>();
        foreach (Vector2Int cell in cells)
        {
            neighbors.Add(new Vector2Int(cell.x - 1, cell.y - 1));  // Top-Left  
            neighbors.Add(new Vector2Int(cell.x, cell.y - 1));      // Top
            neighbors.Add(new Vector2Int(cell.x + 1, cell.y - 1));  // Top-Right
            neighbors.Add(new Vector2Int(cell.x - 1, cell.y));      // Left
            neighbors.Add(new Vector2Int(cell.x + 1, cell.y));      // Right
            neighbors.Add(new Vector2Int(cell.x - 1, cell.y + 1));  // Bottom-Left
            neighbors.Add(new Vector2Int(cell.x, cell.y + 1));      // Bottom
            neighbors.Add(new Vector2Int(cell.x + 1, cell.y + 1));  // Bottom-Right
        }

        foreach (var cell in GetOccupiedPositions())
        {
            neighbors.Remove(cell);
        }
        InteractableCells = neighbors;
        
    }
}

public static class VectorUtils {
    public static Vector2Int ToVector2Int(Vector3 vector) {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
}