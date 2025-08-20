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

    private readonly HashSet<Vector2Int> _blockingViews = new();

    private readonly Dictionary<Race, HashSet<Vector2Int>> _openedCellsD = new();

    private HashSet<Vector2Int> _openedCells => _openedCellsD[Core.Instance.MyRace()];

    private int ViewRadius => Core.ConfigManager.CreaturesParametersConfig.ViewRadius;

    public List<Type> GetDependencies() {
        return new List<Type>() { typeof(SettlersManager), typeof(GridManager), typeof(ConfigManager) };
    }

    public void Init() {
        Core.FogOfWarManager = this;
        if (!gameObject.activeSelf) {
            return;
        }

        _openedCellsD[Race.Robots] = new HashSet<Vector2Int>();
        _openedCellsD[Race.Plants] = new HashSet<Vector2Int>();

        Fill(_blackTilemap, _blackTile);
        Fill(_greyTilemap, _greyTile);

        Core.UI.NetworkReplacement.OnChangeRace += OnChangeRace;
    }

    private void Start() {
        FindAllBlockingViews();
        foreach (SettlerData settler in Core.SettlersManager.MySettlers) {
            OpenAroundMovedSettler(settler);
        }

        Core.GridManager.RefreshAllWalls();
        InvokeRepeating(nameof(FindAllBlockingViews), 0, 1);
    }

    private void OnChangeRace(Race race) {
        ClearAndOpenOpened();
    }

    public bool IsOpened(Vector2Int cell) => _openedCells.Contains(cell);
    public bool IsOpened(Vector3Int cell) => _openedCells.Contains(new Vector2Int(cell.x, cell.y));

    private void FindAllBlockingViews() {
        _blockingViews.Clear();
        Gridable[] blockingView = FindObjectsByType<Gridable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(r => r.IsBlockingView).ToArray();
        foreach (Gridable gridable in blockingView) {
            foreach (Vector2Int pos in gridable.GetOccupiedPositions()) {
                _blockingViews.Add(pos);
            }
        }
    }

    public void OpenAroundMovedSettler(Settler settler) {
        if (!gameObject.activeSelf) {
            return;
        }

        OpenAroundMovedSettler(settler.SettlerData);
    }

    private void OpenAroundMovedSettler(SettlerData settlerData) {
        if (settlerData.Race != Core.Instance.MyRace()) {
            return;
        }

        Vector2Int settlerCell = settlerData.GetCellOnGrid;
        OpenAround(settlerCell, ViewRadius + 2, ViewRadius);
        UpdateGreyFog(settlerCell, ViewRadius + 1);
        RefreshWalls(settlerCell, ViewRadius + 2);
    }

    private void OpenAround(Vector2Int tile, int updateRadius, int viewRadius) {
        int sqrViewRadius = viewRadius * viewRadius;

        for (int i = -updateRadius; i <= updateRadius; i++) {
            for (int j = -updateRadius; j <= updateRadius; j++) {
                Vector2Int tileCoord = new Vector2Int(tile.x + i, tile.y + j);
                if ((tileCoord - tile).sqrMagnitude > sqrViewRadius) {
                    continue;
                }

                if (_openedCells.Contains(tileCoord)) {
                    continue;
                }

                if (!LineOfViewAlgorithm.CanSee(tile, tileCoord, _blockingViews)) {
                    continue;
                }

                Vector3Int tilePos = new Vector3Int(tileCoord.x, tileCoord.y, 0);
                _blackTilemap.SetTile(tilePos, null);
                _openedCells.Add(tileCoord);
            }
        }
    }

    private void RefreshWalls(Vector2Int tile, int updateRadius) {
        for (int i = -updateRadius; i < updateRadius + 1; i++) {
            for (int j = -updateRadius; j < updateRadius + 1; j++) {
                Vector2Int tileCoord = new Vector2Int(tile.x + i, tile.y + j);
                Core.GridManager.RefreshWalls(new Vector3Int(tileCoord.x, tileCoord.y));
            }
        }
    }

    private void UpdateGreyFog(Vector2Int tile, int radius) {
        int sqrViewRadius = ViewRadius * ViewRadius;
        HashSet<Vector2Int> settlerPositions = new(Core.SettlersManager.MySettlers.Select(settler => settler.GetCellOnGrid));

        List<Vector3Int> tilePositions = new();
        List<TileBase> tileTypes = new();

        Vector2Int tileCoord = Vector2Int.zero;
        Vector3Int coord = Vector3Int.zero;

        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                tileCoord.Set(tile.x + i, tile.y + j);

                bool isSeen = settlerPositions.Any(pos => (pos - tileCoord).sqrMagnitude <= sqrViewRadius);
                if (isSeen && !LineOfViewAlgorithm.CanSee(tile, tileCoord, _blockingViews)) {
                    isSeen = false;
                }

                coord.Set(tileCoord.x, tileCoord.y, 0);
                tilePositions.Add(coord);
                tileTypes.Add(isSeen ? null : _greyTile);
            }
        }

        _greyTilemap.SetTiles(tilePositions.ToArray(), tileTypes.ToArray());
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

    private void ClearAndOpenOpened() {
        Fill(_blackTilemap, _blackTile);
        Fill(_greyTilemap, _greyTile);

        foreach (var coord in (_openedCells)) {
            Vector3Int tilePos = new Vector3Int(coord.x, coord.y, 0);
            _blackTilemap.SetTile(tilePos, null);
        }

        foreach (SettlerData settler in Core.SettlersManager.MySettlers) {
            OpenAroundMovedSettler(settler);
        }
    }

    private void OnDestroy() {
        Core.UI.NetworkReplacement.OnChangeRace -= OnChangeRace;
    }
}