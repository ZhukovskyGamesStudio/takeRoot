using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class FogOfWarService : IFogOfWarService, IUpdatable {
    private readonly IUpdateService _updateService;
    private readonly ISettlersService _settlersService;
    private readonly INetworkService _networkService;
    private FogOfWarConfig _config;

    private Tilemap _blackTilemap, _greyTilemap;
    private RectInt _gridSize;

    private float _cooldown;

    public FogOfWarService(IConfigsProvider configsProvider, IUpdateService updateService, ISettlersService settlersService,
        INetworkService networkService) {
        _updateService = updateService;
        _settlersService = settlersService;
        _networkService = networkService;
        _config = configsProvider.FogOfWarConfig;
        var tilemaps = Object.FindObjectsByType<TilemapTypeData>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _blackTilemap = tilemaps.First(t => t.Type == TilemapType.FogOfWarBlack).Tilemap;
        _greyTilemap = tilemaps.First(t => t.Type == TilemapType.FogOfWarGrey).Tilemap;
        _gridSize = _config.FogRect;
        _updateService.Register(this);

        _openedCellsD[Race.Robots] = new HashSet<Vector2Int>();
        _openedCellsD[Race.Plants] = new HashSet<Vector2Int>();

        Fill(_blackTilemap, _config.BlackTile);
        Fill(_greyTilemap, _config.GreyTile);

        //ObsoleteCoreEntryPoint.Instance.OnChangeRace += OnChangeRace;

        FindAllBlockingViews();
        foreach (AI.Settler settler in _settlersService.MySettlers(_networkService.MyRace.Value)) {
            OpenAroundMovedSettler(settler);
        }
        //ObsoleteCoreEntryPoint.GridManager.RefreshAllWalls();
    }

    private readonly HashSet<Vector2Int> _blockingViews = new();

    private readonly Dictionary<Race, HashSet<Vector2Int>> _openedCellsD = new();

    private HashSet<Vector2Int> _openedCells => _openedCellsD[_networkService.MyRace.Value];

    private void OnChangeRace(Race race) {
        ClearAndOpenOpened();
    }

    public bool IsOpened(Vector2Int cell) {
        return _openedCells.Contains(cell);
    }

    public bool IsOpened(Vector3Int cell) {
        return _openedCells.Contains(new Vector2Int(cell.x, cell.y));
    }

    private void FindAllBlockingViews() {
        _blockingViews.Clear();
        Gridable[] blockingView = Object.FindObjectsByType<Gridable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(r => r.IsBlockingView).ToArray();
        foreach (Gridable gridable in blockingView) {
            foreach (Vector2Int pos in gridable.GetOccupiedPositions()) {
                _blockingViews.Add(pos);
            }
        }
    }

    public void OpenAroundMovedSettler(AI.Settler settler) {
        if (AdminManager.IsUnfogingDisabled) {
            return;
        }

        if (settler.Data.names.Race != _networkService.MyRace.Value) {
            return;
        }

        Vector2Int settlerCell = settler.PosOnGrid;
        OpenAround(settlerCell, _config.ViewRadius + 2, _config.ViewRadius);
        UpdateGreyFog(settlerCell, _config.ViewRadius + 1);
        RefreshWalls(settlerCell, _config.ViewRadius + 2);
    }

    private void OpenAround(Vector2Int tile, int updateRadius, float viewRadius) {
        float sqrViewRadius = viewRadius * viewRadius;

        for (int i = -updateRadius; i <= updateRadius; i++) {
            for (int j = -updateRadius; j <= updateRadius; j++) {
                Vector2Int tileCoord = new(tile.x + i, tile.y + j);
                if ((tileCoord - tile).sqrMagnitude > sqrViewRadius) {
                    continue;
                }

                if (_openedCells.Contains(tileCoord)) {
                    continue;
                }

                if (!LineOfViewAlgorithm.CanSee(tile, tileCoord, _blockingViews)) {
                    continue;
                }

                Vector3Int tilePos = new(tileCoord.x, tileCoord.y, 0);
                _blackTilemap.SetTile(tilePos, null);
                _openedCells.Add(tileCoord);
            }
        }
    }

    //TODO fix this method
    private void RefreshWalls(Vector2Int tile, int updateRadius) {
        return;
        for (int i = -updateRadius; i < updateRadius + 1; i++) {
            for (int j = -updateRadius; j < updateRadius + 1; j++) {
                Vector2Int tileCoord = new(tile.x + i, tile.y + j);
                ObsoleteCoreEntryPoint.GridManager.RefreshWalls(new Vector3Int(tileCoord.x, tileCoord.y));
            }
        }
    }

    private void UpdateGreyFog(Vector2Int tile, int radius) {
        int sqrViewRadius = _config.ViewRadius * _config.ViewRadius;
        var myRace = _networkService.MyRace.Value;
        HashSet<Vector2Int> settlerPositions = new(_settlersService.MySettlers(myRace).Select(s => s.PosOnGrid));

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
                tileTypes.Add(isSeen ? null : _config.GreyTile);
            }
        }

        _greyTilemap.SetTiles(tilePositions.ToArray(), tileTypes.ToArray());
    }

    private void Fill(Tilemap tilemap, TileBase tile) {
        RectInt rect = _gridSize;
        Vector2Int min = new(rect.x, rect.y);
        Vector2Int max = new(rect.width, rect.height);
        tilemap.ClearAllTiles();

        // Define the bounds
        BoundsInt bounds = new(min.x, min.y, 0, max.x - min.y + 1, max.y - min.y + 1, 1);
        // Create an array of tiles
        TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y];
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i] = tile;
        }

        // Set the tiles in bulk
        tilemap.SetTilesBlock(bounds, tiles);
    }

    private void ClearAndOpenOpened() {
        Fill(_blackTilemap, _config.BlackTile);
        Fill(_greyTilemap, _config.GreyTile);

        foreach (Vector2Int coord in _openedCells) {
            Vector3Int tilePos = new(coord.x, coord.y, 0);
            _blackTilemap.SetTile(tilePos, null);
        }

        foreach (AI.Settler settler in _settlersService.MySettlers(_networkService.MyRace.Value)) {
            OpenAroundMovedSettler(settler);
        }
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }

    public void Update() {
        _cooldown += Time.deltaTime;

        if (_cooldown >= _config.UpdateFogCooldown) {
            _cooldown = 0;
            FindAllBlockingViews();
            foreach (AI.Settler settler in _settlersService.MySettlers(_networkService.MyRace.Value)) {
                OpenAroundMovedSettler(settler);
            }
        }

        if (AdminManager.FogHControls) {
            if (Input.GetKey(KeyCode.H)) {
                if (Input.GetMouseButton(0)) {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2Int coord = new(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                    OpenAround(coord, 3, 0.9f);
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    Fill(_blackTilemap, _config.BlackTile);
                    Fill(_greyTilemap, _config.GreyTile);
                    _openedCellsD[Race.Robots] = new HashSet<Vector2Int>();
                    _openedCellsD[Race.Plants] = new HashSet<Vector2Int>();
                }
            }
        }
    }
}