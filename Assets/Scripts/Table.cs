using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Table : MonoBehaviour {
    private static readonly int Explored = Animator.StringToHash("Explored");

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private List<GameObject> _objectsPacks;

    [SerializeField]
    private bool _isSelectingRandomViewVariant;

    [SerializeField]
    private InteractableObject _interactableObject;

    public InteractableObject InteractableObject => _interactableObject;

    [SerializeField]
    private List<ResorceData> _dropOnSearch;
    
    [SerializeField]
    private List<ResorceData> _dropOnDestroyed;

    [SerializeField]
    private SpriteRenderer _icon;

    [field: SerializeField]
    public int Health { get; private set; }

    private void Start() {
        if (_isSelectingRandomViewVariant) {
            SelectRandomViewVariant();
        }

        _resorceStorage.Init(2, new List<ResorceData>());

        _interactableObject.AddToPossibleCommands(Command.Search);
        _interactableObject.AddToPossibleCommands(Command.Attack);
        _interactableObject.OnCommandPerformed += OnCommandPerformed;
        _interactableObject.GetInfoFunc = GetInfoData;
    }

    private void OnCommandPerformed(Command obj) {
        if (obj == Command.Search) {
            OnExplored();
            _interactableObject.CancelCommand();
        }

        if (obj == Command.Attack) {
            OnAttacked(1);
            _interactableObject.CancelCommand();
        }
    }

    private void OnAttacked(int damageAmount) {
        Health -= damageAmount;
        if (Health > 0) {
            ShowDamagedAnimation();
        } else {
            OnDestroyed();
            ShowDestroyedAnimation();
        }
    }

    public InfoBookData GetInfoData() {
        var d = new InfoBookData() {
            Icon = _icon.sprite,
            Name = gameObject.name,
            Resources = _resorceStorage.ResorceDatas.ToList()
        };
        return d;
    }
    

    public ResourseStorage _resorceStorage = new ResourseStorage();

    public bool IsStorageActive;
    private void AddToStorage(ResorceData data) {
        _resorceStorage.Add(data);
    }

    private void ShowDamagedAnimation() { }

    private void ShowDestroyedAnimation() { }

    private void SelectRandomViewVariant() {
        int rnd = Random.Range(0, _objectsPacks.Count);
        for (int i = 0; i < _objectsPacks.Count; i++) {
            _objectsPacks[i].SetActive(rnd == i);
        }
    }

    private void OnExplored() {
        _interactableObject.RemoveFromPossibleCommands(Command.Search);
        ResourceManager.SpawnResourcesAround(_dropOnSearch, _interactableObject.GetInteractableSell);
        TriggerExplored();
        IsStorageActive = true;
    }

    private void OnDestroyed() {
        ResourceManager.SpawnResourcesAround(_dropOnDestroyed, _interactableObject.GetCellOnGrid);
        _interactableObject.OnDestroyed();
        Destroy(gameObject);
    }

    private void TriggerExplored() {
        _animator.SetTrigger(Explored);
    }
}