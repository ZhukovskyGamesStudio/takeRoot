using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUnderCleaner : MonoBehaviour {
    [SerializeField]
    private Tilemap _tilemapUnder;

    [SerializeField]
    private bool _gentleClean;

    private Tilemap _tilemap;

    public void ClearUnderTilemap() {
        _tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] tiles = _tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                Vector3Int worldPos = new Vector3Int(bounds.x + x, bounds.y + y, 0);

                if (tiles[x + y * bounds.size.x] == null) {
                    continue;
                }

                if (_gentleClean && IsBorderTile(tiles, bounds.size, x, y)) {
                    continue;
                }

                if (_tilemapUnder.HasTile(worldPos)) {
                    _tilemapUnder.SetTile(worldPos, null);
                }
            }
        }
    }

    private bool IsBorderTile(TileBase[] tiles, Vector3Int size, int x, int y)
    {
        if (x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1)
            return true;

        // Проверяем соседей по сторонам
        if (tiles[(x - 1) + y * size.x] == null) return true;
        if (tiles[(x + 1) + y * size.x] == null) return true;
        if (tiles[x + (y - 1) * size.x] == null) return true;
        if (tiles[x + (y + 1) * size.x] == null) return true;

        // Проверяем диагонали
        if (tiles[(x - 1) + (y - 1) * size.x] == null) return true;
        if (tiles[(x + 1) + (y - 1) * size.x] == null) return true;
        if (tiles[(x - 1) + (y + 1) * size.x] == null) return true;
        if (tiles[(x + 1) + (y + 1) * size.x] == null) return true;

        return false;
    }

}