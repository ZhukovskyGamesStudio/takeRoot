using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {
    public static GridManager Instance;

    [field: SerializeField]
    public Rect GridSize { get; private set; } = new Rect(-10, -10, 10, 10); // Example grid size

    [SerializeField]
    private Tilemap _grassTilemap, _wallsTilemap;

    [SerializeField]
    private TileBase _grassRandomTile;

    [SerializeField]
    private Material _tilemapTransparentMaterial;

    private Camera _mainCamera;

    [SerializeField]
    private float _transparencyRadius = 5.0f;

    private void Awake() {
        Instance = this;
        _mainCamera = Camera.main!;
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

    private void Update() {
        UpdateWallsTransparency();
    }

    private void UpdateWallsTransparency() {
        // Get the world-space cursor position
        Vector3 cursorWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0; // We are working in 2D, so set the Z value to 0

        // Pass the cursor position and radius to the shader
        _tilemapTransparentMaterial.SetVector("_CursorPosition", cursorWorldPosition);
        _tilemapTransparentMaterial.SetFloat("_Radius", _transparencyRadius);
    }

    public void RemoveWall(Vector2Int pos) {
        _wallsTilemap.SetTile((Vector3Int)pos, null);
    }
}