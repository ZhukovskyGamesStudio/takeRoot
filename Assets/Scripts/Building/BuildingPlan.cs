using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private List<ResourceData> _currentResources;

    private Gridable _gridable;
    private Interactable _interactable;
    private bool _isPlaced;

    [SerializeField]
    private Dictionary<ResourceType, int> _reservedResourceAmount = new Dictionary<ResourceType, int>();

    //TODO cancel удаляет объект

    private void Start() {
        _camera = Camera.main;
        _gridable = GetEcsComponent<Gridable>();
        _interactable = GetEcsComponent<Interactable>();
        _interactable.OnCommandPerformed += OnCommandPerformed;

        //TODO создать и наполнить _currentResources пустыми ресурсами из _requiredResources

        foreach (var resource in _requiredResources) {
            _reservedResourceAmount.Add(resource.ResourceType, 0);
        }
    }

    private void Update() {
        if (!_isPlaced) {
            BuildingShadowMouseFollow();
            if (AStarPathfinding.Instance.IsInited()) {
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
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var newPosition = AdjustPositionToGrid(mousePosition);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    private Vector3 AdjustPositionToGrid(Vector3 position) {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);
    }

    private void TryPlaceBuildingPlan() {
        if (!_canPlace)
            return;
        _isPlaced = true;
        _gridable.PositionChanged();
        BuildingManager.Instance.OnPlanPlaced();
    }

    private void CheckObstacles() {
        var occupied = _gridable.GetOccupiedPositions();
        foreach (Vector2Int cell in occupied) {
            if (AStarPathfinding.IsWalkable(cell)) continue;
            _canPlace = false;
            _sprite.color = new Color(255, 0, 0, 100);
            return;
        }

        _canPlace = true;
        _sprite.color = new Color(0, 0, 255, 100);
    }

    private void FormGatherCommands() {
        foreach (ResourceData requiredResource in _requiredResources) {
            var requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
            if (requiredResourceAmount == 0)
                continue;
            List<ResourceView> fitResourcesOnGround = FindFitResourcesOnGround(requiredResource.ResourceType);
            foreach (ResourceView resource in fitResourcesOnGround) {
                if (requiredResourceAmount == 0)
                    break;
                resource.AmountToGather = Mathf.Min(resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[requiredResource.ResourceType] += resource.AmountToGather;
                requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
                CommandData command = new CommandData {
                    Interactable = resource.GetEcsComponent<Interactable>(),
                    Additional = GetEcsComponent<Interactable>(),
                    CommandType = Command.GatherResources,
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                CommandsManagersHolder.Instance.CommandsManager.AddCommandManually(command);
            }

            var requiredResourcesAmount = LeftToBring(requiredResource.ResourceType);
            if (requiredResourceAmount == 0)
                continue;
            List<Storagable> fitStorages = FindFitStorages(requiredResource.ResourceType);
            foreach (Storagable storage in fitStorages) {
                if (requiredResourceAmount == 0)
                    break;
                storage.AmountToGather = Mathf.Min(storage.Resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[requiredResource.ResourceType] += storage.AmountToGather;
                requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
                CommandData command = new CommandData {
                    Interactable = storage.GetComponent<Interactable>(),
                    Additional = GetEcsComponent<Interactable>(),
                    CommandType = Command.GatherResources,
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                CommandsManagersHolder.Instance.CommandsManager.AddCommandManually(command);
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
        if (TryGetComponent(out ResourceView resource))
            _reservedResourceAmount[resource.ResourceType] -= resource.AmountToGather;
        if (TryGetComponent(out Storagable storage))
            _reservedResourceAmount[storage.Resource.ResourceType] -= resource.AmountToGather;
    }

    //public ResourceData GetRequiredResources()
    //{
    //    foreach (var requiredResource in _requiredResources
    //                 .Where(requiredResource => !ResourcesRequirementReached(requiredResource.ResourceType)))
    //        return requiredResource;
    //}

    public List<ResourceView> FindFitResourcesOnGround(ResourceType type) {
        ResourceView[] resourcesOnGround = FindObjectsByType<ResourceView>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        return resourcesOnGround
            .Where(r => r.ResourceType == type && r.Amount != 0 && r.GetEcsComponent<Interactable>().CommandToExecute == null).ToList();
    }

    private List<Storagable> FindFitStorages(ResourceType type) {
        Storagable[] storages = FindObjectsByType<Storagable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        return storages.Where(r =>
            r.Resource.ResourceType == type && r.Resource.Amount != 0 && r.GetComponent<Interactable>().CommandToExecute == null).ToList();
    }

    public void AddResource(ResourceData resource) {
        var currentResource = _currentResources.Find(r => r.ResourceType == resource.ResourceType);
        currentResource.Amount += resource.Amount;
        _reservedResourceAmount[resource.ResourceType] -= resource.Amount;
        if (CanBuild())
            AssignBuildCommand();
    }

    private void AssignBuildCommand() {
        CommandData command = new CommandData() {
            Interactable = _interactable,
            CommandType = Command.Build
        };
        CommandsManagersHolder.Instance.CommandsManager.AddCommandManually(command);
        _interactable.AssignCommand(command);
    }

    private bool CanBuild() {
        foreach (ResourceData currentResource in _currentResources) {
            if (!EnoughResource(currentResource.ResourceType))
                return false;
        }

        return true;
    }

    private void OnCommandPerformed(Command obj) {
        if (obj == Command.Build) {
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
            CommandsManagersHolder.Instance.CommandsManager.RemoveCommandManually(command);
        }
    }
}