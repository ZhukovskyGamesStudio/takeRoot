using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using WorldObjects;

public class ResourceView : ECSEntity {
    [field: SerializeField]
    public int Amount { get; private set; }

    [field: SerializeField]
    public ResourceType ResourceType { get; private set; }

    [Space(20)]
    [SerializeField]
    private TextMeshPro _amountText;

    public bool IsBeingCarried;

    [SerializeField]
    private SpriteRenderer _icon;

    public int AmountToGather;

    private Interactable _interactable;
    public Action<ResourceView> OnResourceViewChanged;

    public ResourceData ResourceData { get; private set; } = new ResourceData();

    public Interactable Interactable => _interactable;

    protected override void Awake() {
        base.Awake();
        _interactable = GetEcsComponent<Interactable>();
        _interactable.GetInfoFunc = GetInfoData;
        _interactable.OnCommandPerformed += OnCommandPerformed;
        _interactable.OnCommandCanceled += OnCommandCanceled;
        _interactable.AddToPossibleCommands(Command.Transport);
        TacticalInteractable tactical = GetEcsComponent<TacticalInteractable>();
        if (tactical != null) {
            tactical.GetInfoFunc = GetInfoData;
        }

        ResourceData.ResourceType = ResourceType;
        SetAmount(Amount);
    }

    private void Update() {
        SetScaleNotInverted();
    }

    private void SetScaleNotInverted() {
        Vector3 scale = transform.localScale;
        if (scale.x < 0) {
            scale.x *= -1;
        }

        if (scale.y < 0) {
            scale.y *= -1;
        }

        transform.localScale = scale;
    }

    private InfoBookData GetInfoData() {
        InfoBookData d = new InfoBookData() {
            Icon = _icon.sprite,
            Name = gameObject.name,
            Resources = new List<ResourceData>()
        };
        return d;
    }

    public void SetAmount(int amount) {
        _amountText.text = amount.ToString();
        Amount = amount;
        ResourceData.Amount = amount;
    }

    private void OnCommandPerformed(CommandData cData) {
        if (cData.CommandType == Command.GatherResources) {
            var resource = new ResourceData() {
                ResourceType = ResourceType,
                Amount = AmountToGather
            };

            if (AmountToGather == Amount) {
                IsBeingCarried = true;
                _interactable.CanSelect = false;
                GetEcsComponent<Networkable>().ChangeParent(cData.Settler.ResourceHolder);
                cData.CommandType = Command.Delivery;
                cData.AdditionalData = new DeliveryCommandData() {
                    TargetPlan = _interactable.CommandToExecute.Additional.GetComponent<BuildingPlan>()
                };
                AmountToGather = 0;
                CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(cData);
            } else {
                var position = cData.Settler.GetCellOnGrid;
                var resourceToGather = ResourceManager.SpawnResourceAt(resource, position);
                resourceToGather.IsBeingCarried = true;
                resourceToGather._interactable.CanSelect = false;
                AmountToGather = 0;
                SetAmount(Amount - resource.Amount);
                CommandData command = new CommandData() {
                    Interactable = resourceToGather.Interactable,
                    Additional = cData.Additional,
                    AdditionalData = new DeliveryCommandData() {
                        TargetPlan = cData.Additional.GetComponent<BuildingPlan>()
                    },
                    CommandType = Command.Delivery,
                    Settler = cData.Settler
                };
                cData.TriggerCancel?.Invoke();
                _interactable.CancelCommand();
                resourceToGather._interactable.AssignCommand(command);
                resourceToGather.GetEcsComponent<Networkable>()
                    .ChangeParent(resourceToGather._interactable.CommandToExecute.Settler.ResourceHolder);
                CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(resourceToGather._interactable.CommandToExecute);
            }
            return;
        }
        if (cData.CommandType == Command.GatherResourcesForCraft)
        {
            var resource = new ResourceData() {
                ResourceType = ResourceType,
                Amount = AmountToGather
            };

            if (AmountToGather == Amount) {
                IsBeingCarried = true;
                _interactable.CanSelect = false;
                GetEcsComponent<Networkable>().ChangeParent(cData.Settler.ResourceHolder);
                CommandData command = new CommandData()
                {
                    Interactable = _interactable,
                    Additional = cData.Additional,
                    AdditionalData = new DeliveryToCraftCommandData()
                    {
                        CraftingStation = cData.Additional.GetComponent<CraftingStationable>()
                    },
                    CommandType = Command.DeliveryForCraft,
                    Settler = cData.Settler
                };
                AmountToGather = 0;
                CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(command);
            } else {
                var position = cData.Settler.GetCellOnGrid;
                var resourceToGather = ResourceManager.SpawnResourceAt(resource, position);
                resourceToGather.IsBeingCarried = true;
                resourceToGather._interactable.CanSelect = false;
                AmountToGather = 0;
                SetAmount(Amount - resource.Amount);
                CommandData command = new CommandData() {
                    Interactable = resourceToGather.Interactable,
                    Additional = cData.Additional,
                    AdditionalData = new DeliveryToCraftCommandData() {
                        CraftingStation = cData.Additional.GetComponent<CraftingStationable>()
                    },
                    CommandType = Command.DeliveryForCraft,
                    Settler = cData.Settler
                };
                cData.TriggerCancel?.Invoke();
                _interactable.CancelCommand();
                resourceToGather._interactable.AssignCommand(command);
                resourceToGather.GetEcsComponent<Networkable>()
                    .ChangeParent(resourceToGather._interactable.CommandToExecute.Settler.ResourceHolder);
                CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(resourceToGather._interactable.CommandToExecute);
            }
            return;
        }

        if (cData.CommandType == Command.Delivery) {
            DeliveryCommandData deliveryData = (DeliveryCommandData)cData.AdditionalData;
            deliveryData.TargetPlan.GetComponent<BuildingPlan>().AddResource(ResourceData);
            _interactable.CancelCommand();
            OnDestroy();
            return;
        }

        if (cData.CommandType == Command.DeliveryForCraft)
        {
            DeliveryToCraftCommandData deliveryData = (DeliveryToCraftCommandData)cData.AdditionalData;
            deliveryData.CraftingStation.GetComponent<CraftingStationable>().AddResourceToStorage(ResourceData);
            _interactable.CancelCommand();
            OnDestroy();
            return;
        }

        if (cData.CommandType == Command.Transport) {
            IsBeingCarried = true;
            _interactable.CanSelect = false;
            cData.CommandType = Command.Store;
            Storagable storage = ResourceManager.Instance.FindClosestAvailableStorage(ResourceData, _interactable.GetInteractableCell);
            cData.AdditionalData = new StoreCommandData() {
                TargetStorage = storage,
                Resource = this
            };
            CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(cData);
            GetEcsComponent<Networkable>().ChangeParent(cData.Settler.ResourceHolder);
            transform.localPosition = Vector3.zero;
            if (cData.PlannedCommandView != null) {
                cData.PlannedCommandView.Release();
            }

            return;
        }

        if (cData.CommandType == Command.Store) {
            StoreCommandData storeData = (StoreCommandData)cData.AdditionalData;
            storeData.TargetStorage.AddResource(ResourceData);
            _interactable.CancelCommand();
            SetAmount(ResourceData.Amount);
            if (ResourceData.Amount == 0)
                OnDestroy();
        }
    }

    private void OnDestroy()
    {
        ResourceManager.ClearResourceView(this);
        _interactable.OnDestroyed();
    }


    private void OnCommandCanceled(CommandData type) {
        DropOnGround();
    }

    public void DropOnGround() {
        if (!IsBeingCarried) {
            return;
        }

        IsBeingCarried = false;
        _interactable.CanSelect = true;
        _interactable.Gridable.PositionChanged();
        GetEcsComponent<Networkable>().ChangeParent(ResourceManager.Instance.ResourcesHolder.gameObject);
    }
}