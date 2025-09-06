using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

public class BuildingBlueprint : NetworkBehaviour, IUpdatable {
    public Dictionary<ResourceType, int> RequiredResources = new();
    public Dictionary<ResourceType, int> ReservedRequiredResources = new();
    public Dictionary<ResourceType, int> ResourceStorage = new();
    public AI.Settler Builder;
    public Vector3? InteractionPos => _gridObject.GetNeighborFreeTile();
    private CommandTarget _buildingPrefab;

    [FormerlySerializedAs("Builded")]
    public bool WasBuilded;

    public bool IsPlaced;
    private bool _canPlace;
    private int _buildPoints, _neededBuildPoints;

    [SerializeField]
    private SpriteRenderer _sprite;

    [SerializeField]
    private CommandTarget _commandTarget;

    [SerializeField]
    private Progress _progressBar;

    [SerializeField]
    private Collider2D _clickCollider;

    private IUpdateService _update;
    private Camera _camera;
    private IGridService _grid;
    private GridObject _gridObject;
    private IBuildingService _buildingService;
    private IInputService _input;
    private Race _placedByRace = Race.None;
    private INetworkService _networkService;

    private void Awake() {
        _networkService = ServiceLocator.Container.Single<INetworkService>();
        _update = ServiceLocator.Container.Single<IUpdateService>();
        _grid = ServiceLocator.Container.Single<IGridService>();
        _buildingService = ServiceLocator.Container.Single<IBuildingService>();
        _input = ServiceLocator.Container.Single<IInputService>();
        _gridObject = GetComponent<GridObject>();
        _camera = Camera.main;
    }

    [ClientRpc]
    public void InitClientRpc(string biuldingName, Race placedByRace) {
        _placedByRace = placedByRace;
        IsPlaced = false;
        var cnfg = ServiceLocator.Container.Single<IConfigsProvider>().BuildingsBlueprintsConfigs.First(c => c.mainInfo.Name == biuldingName);
        Init(cnfg);
    }

    public void Init(BuildingRecipeConfig config) {
        _commandTarget.Data.MainInfoData = config.mainInfo;
        _progressBar.ProgressData.Needed = config.RequiredBuildPoints;

        _neededBuildPoints = config.RequiredBuildPoints;

        foreach (var kvp in config.Ingridients) {
            var type = kvp.Key;
            var amount = kvp.Value;
            RequiredResources.Add(type, amount);
            ReservedRequiredResources.Add(type, 0);
            ResourceStorage.Add(type, 0);
        }

        _gridObject.MultiplyGridOffset.x = config.Footprint.x - 1;
        _gridObject.MultiplyGridOffset.y = config.Footprint.y - 1;
        var boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(config.Footprint.x, config.Footprint.y + 1);
        boxCollider2D.offset = new Vector2(0.5f, 0.5f);
        _sprite.sprite = config.mainInfo.Icon;

        _sprite.transform.localPosition = config.BuildingPrefab.transform.Find("View").localPosition;

        _buildingPrefab = config.BuildingPrefab;
        _gridObject.Obstacle = _buildingPrefab.GetComponent<GridObject>().Obstacle;
        //_update.Register(this);
    }

    public ResourceType GetRequiredResource() {
        foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None || !RequiredResources.ContainsKey(type)) continue;

            var amount = RequiredResources[type] - ReservedRequiredResources[type] - ResourceStorage[type];
            if (amount > 0) {
                return type;
            }
        }

        return ResourceType.None;
    }

    public bool CanBuild() {
        var canBuild = true;
        foreach (var kvp in RequiredResources) {
            var type = kvp.Key;
            var amount = kvp.Value;
            if (ResourceStorage[type] < amount) {
                canBuild = false;
            }
        }

        return canBuild;
    }

    public void Update() {
        if (IsPlaced) {
            return;
        }

        if (IsHost) {
            CheckObstacles();
        }

        if (_networkService.MyRace.Value != _placedByRace) {
            return;
        }

        BuildingShadowMouseFollow();

        if (_input.GetMouseButtonDown(MouseButton.Left)) {
            TryPlaceBuildingBlueprint();
        }
    }

    private void BuildingShadowMouseFollow() {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition = AdjustPositionToGrid(mousePosition);
        newPosition.z = transform.position.z;
        MoveToMouseServerRpc(newPosition);
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveToMouseServerRpc(Vector3 newPosition) {
        UpdatePosClientRpc(newPosition);
        _gridObject.UpdatePosition();
    }

    [ClientRpc]
    private void UpdatePosClientRpc(Vector3 newPosition) {
        transform.position = newPosition;
    }

    private Vector3 AdjustPositionToGrid(Vector3 position) {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);
    }

    private void CheckObstacles() {
        List<Vector3> occupied = _gridObject.GetObjectPositions();
        foreach (Vector3 cell in occupied) {
            if (_grid.OnMap(cell) && !_grid.IsOccupiedPos(cell)) {
                continue;
            }

            SetCanPlaceClientRpc(false);
            return;
        }

        SetCanPlaceClientRpc(true);
    }

    [ClientRpc]
    private void SetCanPlaceClientRpc(bool canPlace) {
        _canPlace = canPlace;
        _sprite.color = canPlace ? new Color(0, 0, 255, 100) : new Color(255, 0, 0, 100);
    }

    private void TryPlaceBuildingBlueprint() {
        if (!_canPlace) {
            Debug.Log("Can't place here");
            return;
        }

        Debug.Log("Placing here");
        Place();
    }

    private void Place() {
        PlaceServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlaceServerRpc() {
        _update.Unregister(this);
        _buildingService.PlaceBlueprint(this);
        MoveToMouseServerRpc(transform.position);
        SetPlacedClientRpc();

        if (AdminManager.IsInstaBuild) {
            _buildPoints += 100000;
            Build();
        }
    }

    [ClientRpc]
    private void SetPlacedClientRpc() {
        IsPlaced = true;
        _clickCollider.enabled = true;
    }

    public void CancelPlacement() {
        CancelPlacementServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CancelPlacementServerRpc() {
        _update.Unregister(this);
        _buildingService.CancelBlueprint(this);
        Destroy(gameObject);
    }

    public void StoreResource(ResourceType type, int amount) {
        ResourceStorage[type] += amount;
        ReservedRequiredResources[type] -= amount;
    }

    public void Build() {
        _buildPoints++;
        _progressBar.ProgressData.Progress.Value = _buildPoints;
        if (_buildPoints < _neededBuildPoints) {
            return;
        }

        _buildingService.Build(this);
        WasBuilded = true;
        //TODO: move instantiate to service
        _networkService.InstantiateAndSpawn(_buildingPrefab, transform.position);
        if (_gridObject.Obstacle) {
            _gridObject.OccupyTiles();
        }

        Destroy(gameObject);
    }

    public void Dispose() {
        _update.Unregister(this);
    }
}