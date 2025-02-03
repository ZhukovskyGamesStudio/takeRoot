using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWarManager : MonoBehaviour, IInitableInstance {
    [SerializeField]
    private Tilemap _blackTilemap, _greyTilemap;

    [SerializeField]
    private TileBase _blackTile, _greyTile;

    private readonly HashSet<Vector2Int> _openedCells = new();

    private int ViewRadius => Core.ConfigManager.CreaturesParametersConfig.ViewRadius;

    public List<Type> GetDependencies() {
        return new List<Type>() { typeof(SettlersManager), typeof(GridManager), typeof(ConfigManager) };
    }

    public void Init() {
        Core.FogOfWarManager = this;
        Fill(_blackTilemap, _blackTile);
        Fill(_greyTilemap, _greyTile);
        foreach (Settler settler in Core.SettlersManager.MySettlers) {
            OpenAroundMovedSettler(settler);
        }
    }

    public void OpenAroundMovedSettler(Settler settler) {
        if (settler.Race != Core.Instance.MyRace()) {
            return;
        }

        Vector2Int settlerCell = settler.GetCellOnGrid;
        OpenAround(settlerCell, ViewRadius);
        UpdateGreyFog(settlerCell, ViewRadius + 1);
    }

    private void OpenAround(Vector2Int tile, int radius) {
        for (int i = -radius; i < radius + 1; i++) {
            for (int j = -radius; j < radius + 1; j++) {
                Vector2Int tileCoord = new Vector2Int(tile.x + i, tile.y + j);
                if (_openedCells.Contains(tileCoord)) {
                    continue;
                }

                if (!(Vector2Int.Distance(tile, tileCoord) <= radius)) {
                    continue;
                }

                _blackTilemap.SetTile(new Vector3Int(tileCoord.x, tileCoord.y, 0), null);
                _openedCells.Add(tileCoord);
            }
        }
    }

    private void UpdateGreyFog(Vector2Int tile, int radius) {
        for (int i = -radius; i < radius + 1; i++) {
            for (int j = -radius; j < radius + 1; j++) {
                Vector2Int tileCoord = new Vector2Int(tile.x + i, tile.y + j);

                bool isSeen = Core.SettlersManager.MySettlers.Any(
                    settler => Vector2Int.Distance(settler.GetCellOnGrid, tileCoord) <= ViewRadius);

                TileBase tileBase = isSeen ? null : _greyTile;
                _greyTilemap.SetTile(new Vector3Int(tileCoord.x, tileCoord.y, 0), tileBase);
            }
        }
    }

    private void Fill(Tilemap tilemap, TileBase tile) {
        Rect rect = Core.GridManager.GridSize;
        Vector2Int min = new Vector2Int((int)rect.x, (int)rect.y);
        Vector2Int max = new Vector2Int((int)rect.width, (int)rect.height);
        tilemap.ClearAllTiles();

        // Define the bounds
        BoundsInt bounds = new BoundsInt(min.x, min.y, 0, max.x - min.y + 1, max.y - min.y + 1, 1);
        // Create an array of tiles
        TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y];
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i] = tile;
        }

        // Set the tiles in bulk
        tilemap.SetTilesBlock(bounds, tiles);
    }
}