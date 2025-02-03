using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour, IInitableInstance {
    private static readonly int CursorPosition = Shader.PropertyToID("_CursorPosition");
    private static readonly int Radius = Shader.PropertyToID("_Radius");

    [field: SerializeField]
    public Rect GridSize { get; private set; } = new Rect(-10, -10, 10, 10); // Example grid size

    [SerializeField]
    private Tilemap _grassTilemap, _wallsTilemap;

    [SerializeField]
    private TileBase _grassRandomTile;

    [SerializeField]
    private Material _tilemapTransparentMaterial;

    [SerializeField]
    private float _transparencyRadius = 5.0f;

    private Camera _mainCamera;

    private void Update() {
        UpdateWallsTransparency();
    }

    private void OnApplicationQuit() {
        _tilemapTransparentMaterial.SetVector(CursorPosition, Vector3.zero);
        _tilemapTransparentMaterial.SetFloat(Radius, 0);
    }

    public void Init() {
        Core.GridManager = this;
        _mainCamera = Camera.main!;
        FillGrass();
    }

    public void RefreshWalls(Vector3Int cell) {
        _wallsTilemap.RefreshTile(cell);
    }

    public void RefreshAllWalls() {
        _wallsTilemap.RefreshAllTiles();
    }

    private void FillGrass() {
        Rect rect = GridSize;
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

    private void UpdateWallsTransparency() {
        // Get the world-space cursor position
        Vector3 cursorWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0; // We are working in 2D, so set the Z value to 0

        // Pass the cursor position and radius to the shader
        _tilemapTransparentMaterial.SetVector(CursorPosition, cursorWorldPosition);
        _tilemapTransparentMaterial.SetFloat(Radius, _transparencyRadius);
    }

    public void RemoveWall(Vector2Int pos) {
        _wallsTilemap.SetTile((Vector3Int)pos, null);
    }
}