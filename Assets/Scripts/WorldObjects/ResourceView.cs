using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

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

    private Interactable _interactable;

    public ResourceData ResourceData { get; private set; } = new ResourceData();

    protected override void Awake() {
        base.Awake();
        _interactable = GetEcsComponent<Interactable>();
        _interactable.GetInfoFunc = GetInfoData;
        _interactable.OnCommandPerformed += OnCommandPerformed;
        _interactable.OnCommandCanceled += OnCommandCanceled;
        _interactable.AddToPossibleCommands(Command.Transport);
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

    private void OnCommandPerformed(Command obj) {
        if (obj == Command.Transport) {
            IsBeingCarried = true;
            _interactable.CanSelect = false;
            _interactable.CommandToExecute.CommandType = Command.Store;
            //var storage = ResourceManager.Instance.FindEmptyStorageForResorce(ResourceData);
            //Interactable interactableStorage = storage.GetEcsComponent<Interactable>();
            //_interactable.CommandToExecute.Additional = interactableStorage;
            CommandsManager.Instance.AddSubsequentCommand(_interactable.CommandToExecute);
            GetEcsComponent<Networkable>().ChangeParent(_interactable.CommandToExecute.Settler.ResourceHolder);
            transform.localPosition = Vector3.zero;
            if (_interactable.CommandToExecute.PlannedCommandView != null) {
                _interactable.CommandToExecute.PlannedCommandView.Release();
            }
        }

        if (obj == Command.Store) {
            _interactable.CommandToExecute.Additional.GetComponent<Storagable>().AddResource(ResourceData);
            _interactable.CancelCommand();
            SetAmount(ResourceData.Amount);
            if (ResourceData.Amount == 0)
                _interactable.OnDestroyed();
        }
    }

    private void OnCommandCanceled(Command type) {
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