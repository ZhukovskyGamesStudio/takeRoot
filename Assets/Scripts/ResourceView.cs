using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceView : MonoBehaviour {
    public int Amount { get; private set; }

    [field: SerializeField]
    public ResourceType ResourceType { get; private set; }

    [Space(20)]
    [SerializeField]
    private TextMeshPro _amountText;

    public bool IsBeingCarried;

    [SerializeField]
    private InteractableObject _interactableObject;

    [SerializeField]
    private SpriteRenderer _icon;

    private ResorceData _resorceData = new ResorceData();

    private void Start() {
        _resorceData.ResourceType = ResourceType;
        _interactableObject.AddToPossibleCommands(Command.Transport);
        _interactableObject.OnCommandPerformed += OnCommandPerformed;
        _interactableObject.GetInfoFunc = GetInfoData;
    }
    
    public InfoBookData GetInfoData() {
        var d = new InfoBookData() {
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
        if (obj == Command.Transport ) {
            IsBeingCarried = true;
            _interactableObject.CommandToExecute.CommandType = Command.Store;
            _interactableObject.CommandToExecute.AdditionalObject = ResourceManager.Instance.FindEmptyStorageForResorce(_resorceData).InteractableObject;
            CommandsManager.Instance.AddSubsequentCommand(_interactableObject.CommandToExecute);
            transform.SetParent(_interactableObject.CommandToExecute.Settler.transform);
            if (_interactableObject.CommandToExecute.PlannedCommandView != null) {
                _interactableObject.CommandToExecute.PlannedCommandView.Release();
            }
        }

        if (obj == Command.Store) {
            IsBeingCarried = false;
            _interactableObject.CommandToExecute.AdditionalObject.GetComponent<Table>()._resorceStorage.Add(_resorceData);
            _interactableObject.CancelCommand();
            _interactableObject.OnDestroyed();
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