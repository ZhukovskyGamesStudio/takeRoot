using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {
    public static GridManager Instance;

    [field: SerializeField]
    public Rect GridSize { get; private set; } = new Rect(-10, -10, 10, 10); // Example grid size

    [SerializeField]
    private Tilemap _grassTilemap;

    [SerializeField]
    private TileBase _grassRandomTile;

    private void Awake() {
        Instance = this;
        FillGrass();
    }

    private void FillGrass() {
        Rect rect = Instance.GridSize;
        Vector2Int min = new Vector2Int((int)rect.x, (int)rect.y);
        Vector2Int max = new Vector2Int((int)rect.width, (int)rect.height);
        _grassTilemap.ClearAllTiles();

        // Define the bounds
        BoundsInt bounds = new BoundsInt(min.x, min.y, 0, max.x - min.y + 1, max.y - min.y + 1, 1);
        // Create an array of tiles
        TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y];
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i] = _grassRandomTile;
        }

        // Set the tiles in bulk
        _grassTilemap.SetTilesBlock(bounds, tiles);
    }
}