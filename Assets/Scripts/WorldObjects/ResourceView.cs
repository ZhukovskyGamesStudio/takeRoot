using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceView : ECSEntity {
    public int Amount { get; private set; }

    [field: SerializeField]
    public ResourceType ResourceType { get; private set; }

    [Space(20)]
    [SerializeField]
    private TextMeshPro _amountText;

    public bool IsBeingCarried;

    private Interactable _interactable;

    [SerializeField]
    private SpriteRenderer _icon;

    private ResorceData _resorceData = new ResorceData();

    protected override void Awake() {
        base.Awake();
        _interactable = GetEcsComponent<Interactable>();
        _interactable.GetInfoFunc = GetInfoData;
        _interactable.OnCommandPerformed += OnCommandPerformed;
        _interactable.AddToPossibleCommands(Command.Transport);
        _resorceData.ResourceType = ResourceType;
    }

    private InfoBookData GetInfoData() {
        InfoBookData d = new InfoBookData() {
            Icon = _icon.sprite,
            Name = gameObject.name,
            Resources = new List<ResorceData>()
        };
        return d;
    }

    public void SetAmount(int amount) {
        _amountText.text = amount.ToString();
        Amount = amount;
        _resorceData.Amount = amount;
    }

    private void OnCommandPerformed(Command obj) {
        if (obj == Command.Transport) {
            IsBeingCarried = true;
            _interactable.CommandToExecute.CommandType = Command.Store;
            _interactable.CommandToExecute.Additional =
                ResourceManager.Instance.FindEmptyStorageForResorce(_resorceData).GetEcsComponent<Interactable>();
            CommandsManager.Instance.AddSubsequentCommand(_interactable.CommandToExecute);
            transform.SetParent(_interactable.CommandToExecute.Settler.transform);
            if (_interactable.CommandToExecute.PlannedCommandView != null) {
                _interactable.CommandToExecute.PlannedCommandView.Release();
            }
        }

        if (obj == Command.Store) {
            IsBeingCarried = false;
            _interactable.CommandToExecute.Additional.GetComponent<Table>().ResorceStorage.Add(_resorceData);
            _interactable.CancelCommand();
            _interactable.OnDestroyed();
            Destroy(gameObject);
        }
    }
}

[SerializeField]
public enum ResourceType {
    None = 0,
    Planks = 1,
    MetalScraps = 2,
}