using System;
using UnityEditor.UIElements;
using UnityEngine;

public class Storagable : ECSComponent {
    [field: SerializeField]
    public int MaxStack { get; private set; } = 10;

    //TODO тут должен быть список
    [field: SerializeField]
    public ResourceData Resource { get; private set; } = ResourceData.Empty;

    public int AmountToGather;
    private Interactable _interactable;

    public Interactable Interactable => _interactable;

    private void Awake() {
        _interactable = GetComponent<Interactable>();
        _interactable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(CommandData cData) {
        if (cData.CommandType == Command.GatherResources) {
            var resource = new ResourceData() {
                ResourceType = Resource.ResourceType,
                Amount = AmountToGather
            };
            var position = _interactable.CommandToExecute.Settler.GetCellOnGrid;
            var resourceToGather = ResourceManager.SpawnResourceAt(resource, position);
            resourceToGather.IsBeingCarried = true;
            resourceToGather.Interactable.CanSelect = false;
            AmountToGather = 0;
            Resource.Amount -= resource.Amount;
            CommandData command = new CommandData() {
                Interactable = resourceToGather.Interactable,
                Additional = _interactable.CommandToExecute.Additional,
                CommandType = Command.Delivery,
                AdditionalData = new DeliveryCommandData() {
                    TargetPlan = _interactable.CommandToExecute.Additional.GetComponent<BuildingPlan>()
                },
                Settler = _interactable.CommandToExecute.Settler
            };
            _interactable.CommandToExecute.TriggerCancel?.Invoke();
            _interactable.CancelCommand();
            resourceToGather.Interactable.AssignCommand(command);
            resourceToGather.GetEcsComponent<Networkable>().ChangeParent(resourceToGather.Interactable.CommandToExecute.Settler.ResourceHolder);
            CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(resourceToGather.Interactable.CommandToExecute);
        }
    }

    public void AddResource(ResourceData resourceData) {
        if (resourceData.ResourceType != Resource.ResourceType && !IsEmpty())
            return;
        if (IsEmpty())
            Resource.ResourceType = resourceData.ResourceType;

        var canAdd = MaxStack - Resource.Amount;
        if (resourceData.Amount <= canAdd) {
            Resource.Amount += resourceData.Amount;
            resourceData.Amount -= resourceData.Amount;
        } else {
            Resource.Amount += canAdd;
            resourceData.Amount -= canAdd;
        }
    }

    //Work in progress
    public ResourceData RemoveResource(ResourceData resourceData) {
        if (IsEmpty())
            return null;
        if (resourceData.ResourceType != Resource.ResourceType)
            return null;
        if (Resource.Amount - resourceData.Amount < 0)
            return null;

        int canRemove = Math.Min(resourceData.Amount, Resource.Amount);
        Resource.Amount -= resourceData.Amount;
        if (Resource.Amount == 0)
            Resource.ResourceType = ResourceType.None;

        var removedResource = new ResourceData {
            ResourceType = Resource.ResourceType,
            Amount = resourceData.Amount
        };

        if (IsEmpty())
            Resource.ResourceType = ResourceType.None;

        return removedResource;
    }

    public bool CanRemove(ResourceData resourceData) {
        return !IsEmpty() && resourceData.ResourceType == Resource.ResourceType;
    }

    public bool CanStore(ResourceData resourceData) {
        if (IsEmpty()) {
            return true;
        }

        return resourceData.ResourceType == Resource.ResourceType && !IsFull();
    }

    public bool HasResources(ResourceData resourceData) {
        return resourceData.ResourceType == Resource.ResourceType && resourceData.Amount <= Resource.Amount;
    }

    private bool IsFull() {
        return Resource.Amount == MaxStack;
    }

    private bool IsEmpty() {
        return Resource.Amount == 0;
    }

    public override int GetDependancyPriority() {
        return 0;
    }

    public override void Init(ECSEntity entity) { }
}