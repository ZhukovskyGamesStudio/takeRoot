using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Obsolete]
public class BuildingPlan : ECSEntity {
    public string Name;
    public SpriteRenderer Icon;

    [SerializeField]
    private GameObject _buildingPrefab;

    [SerializeField]
    private Transform _grid;

    [SerializeField]
    private SpriteRenderer _sprite;

    [SerializeField]
    private int _buildPoints;

    [SerializeField]
    private List<ResourceData> _requiredResources;

    [SerializeField]
    private List<CommandData> _activeGatherCommands;

    private Camera _camera;
    private bool _canBuild;

    private bool _canPlace;

    private List<ResourceData> _currentResources = new();

    private Gridable _gridable;
    private Interactable _interactable;
    private bool _isPlaced;

    [SerializeField]
    private Dictionary<ResourceType, int> _reservedResourceAmount = new();

    public Interactable Interactable => _interactable;

    //TODO cancel удаляет объект

    private void Start() {
        _camera = Camera.main;
        _gridable = GetEcsComponent<Gridable>();
        _interactable = GetEcsComponent<Interactable>();
        _interactable.OnCommandPerformed += OnCommandPerformed;

        //TODO создать и наполнить _currentResources пустыми ресурсами из _requiredResources

        foreach (ResourceData resource in _requiredResources) {
            _reservedResourceAmount.Add(resource.ResourceType, 0);
            _currentResources.Add(new ResourceData {
                Amount = 0,
                ResourceType = resource.ResourceType
            });
        }
    }

    private void Update() {
        if (!_isPlaced) {
            BuildingShadowMouseFollow();
            if (ObsoleteCoreEntryPoint.AStarPathfinding.IsInited()) {
                CheckObstacles();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                TryPlaceBuildingPlan();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                OnDestroyed();
                _interactable.OnDestroyed();
            }
        }

        if (_isPlaced) {
            FormGatherCommands();
        }
    }

    private void BuildingShadowMouseFollow() {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition = AdjustPositionToGrid(mousePosition);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    private Vector3 AdjustPositionToGrid(Vector3 position) {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);
    }

    private void TryPlaceBuildingPlan() {
        if (!_canPlace) {
            return;
        }

        _isPlaced = true;
        _gridable.PositionChanged();
        ObsoleteCoreEntryPoint.BuildingManager.OnPlanPlaced();
    }

    private void CheckObstacles() {
        List<Vector2Int> occupied = _gridable.GetOccupiedPositions();
        foreach (Vector2Int cell in occupied) {
            if (AStarPathfinding.IsWalkable(cell)) {
                continue;
            }

            _canPlace = false;
            _sprite.color = new Color(255, 0, 0, 100);
            return;
        }

        _canPlace = true;
        _sprite.color = new Color(0, 0, 255, 100);
    }

    private void FormGatherCommands() {
        foreach (ResourceData requiredResource in _requiredResources) {
            int requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
            if (requiredResourceAmount == 0) {
                continue;
            }

            List<ResourceView> fitResourcesOnGround = ResourceManager.FindFitResourcesOnGround(requiredResource.ResourceType);
            foreach (ResourceView resource in fitResourcesOnGround) {
                if (requiredResourceAmount == 0) {
                    break;
                }

                resource.AmountToGather = Mathf.Min(resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[requiredResource.ResourceType] += resource.AmountToGather;
                requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
                CommandData command = new() {
                    Interactable = resource.GetEcsComponent<Interactable>(),
                    Additional = GetEcsComponent<Interactable>(),
                    CommandType = Command.GatherResources
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                ObsoleteCoreEntryPoint.CommandsManagersHolder.CommandsManager.AddCommandManually(command);
            }

            int requiredResourcesAmount = LeftToBring(requiredResource.ResourceType);
            if (requiredResourceAmount == 0) {
                continue;
            }

            List<Storagable> fitStorages = ResourceManager.FindFitStorages(requiredResource.ResourceType);
            foreach (Storagable storage in fitStorages) {
                if (requiredResourceAmount == 0) {
                    break;
                }

                storage.AmountToGather = Mathf.Min(storage.Resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[requiredResource.ResourceType] += storage.AmountToGather;
                requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
                CommandData command = new() {
                    Interactable = storage.GetComponent<Interactable>(),
                    Additional = GetEcsComponent<Interactable>(),
                    CommandType = Command.GatherResources
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                ObsoleteCoreEntryPoint.CommandsManagersHolder.CommandsManager.AddCommandManually(command);
            }
        }
    }

    private bool EnoughResource(ResourceType type) {
        return _requiredResources.Find(r => r.ResourceType == type).Amount == _currentResources.Find(r => r.ResourceType == type).Amount;
    }

    private int LeftToBring(ResourceType type) {
        return _requiredResources.Find(r => r.ResourceType == type).Amount - _currentResources.Find(r => r.ResourceType == type).Amount -
               _reservedResourceAmount[type];
    }

    public bool ResourcesRequirementReached(ResourceType type) {
        return _requiredResources.Find(r => r.ResourceType == type).Amount == _currentResources.Find(r => r.ResourceType == type).Amount;
    }

    private void CancelGatherCommand(CommandData command) {
        _activeGatherCommands.Remove(command);
        if (TryGetComponent(out ResourceView resource)) {
            _reservedResourceAmount[resource.ResourceType] -= resource.AmountToGather;
        }

        if (TryGetComponent(out Storagable storage)) {
            _reservedResourceAmount[storage.Resource.ResourceType] -= resource.AmountToGather;
        }
    }

    //public ResourceData GetRequiredResources()
    //{
    //    foreach (var requiredResource in _requiredResources
    //                 .Where(requiredResource => !ResourcesRequirementReached(requiredResource.ResourceType)))
    //        return requiredResource;
    //}

    public void AddResource(ResourceData resource) {
        ResourceData currentResource = _currentResources.Find(r => r.ResourceType == resource.ResourceType);
        currentResource.Amount += resource.Amount;
        _reservedResourceAmount[resource.ResourceType] -= resource.Amount;
        if (CanBuild()) {
            AssignBuildCommand();
        }
    }

    private void AssignBuildCommand() {
        CommandData command = new() {
            Interactable = _interactable,
            CommandType = Command.Build
        };
        ObsoleteCoreEntryPoint.CommandsManagersHolder.CommandsManager.AddCommandManually(command);
        _interactable.AssignCommand(command);
    }

    private bool CanBuild() {
        foreach (ResourceData currentResource in _currentResources) {
            if (!EnoughResource(currentResource.ResourceType)) {
                return false;
            }
        }

        return true;
    }

    private void OnCommandPerformed(CommandData obj) {
        if (obj.CommandType == Command.Build) {
            int fakeBuildPoints = 1;
            _buildPoints -= fakeBuildPoints;
            if (_buildPoints == 0) {
                OnBuildCompleted();
                _interactable.CancelCommand();
            }
        }
    }

    private void OnBuildCompleted() {
        Instantiate(_buildingPrefab, transform.position, Quaternion.identity);
        _interactable.OnDestroyed();
    }

    private void OnDestroyed() {
        //TODO надо выбрасывать принесённые ресурсы на землю
        foreach (CommandData command in _activeGatherCommands.ToList()) {
            command.Interactable.CancelCommand();
            ObsoleteCoreEntryPoint.CommandsManagersHolder.CommandsManager.RemoveCommandManually(command);
        }
    }
}