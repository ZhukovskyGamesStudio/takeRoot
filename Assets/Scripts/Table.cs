using System;
using System.Collections.Generic;
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

    private void Start() {
        if (_isSelectingRandomViewVariant) {
            SelectRandomViewVariant();
        }

        _interactableObject.AddToPossibleCommands(Command.Search);
        _interactableObject.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(Command obj) {
        if (obj == Command.Search) {
            OnExplored();
        }
    }

    private void SelectRandomViewVariant() {
        int rnd = Random.Range(0, _objectsPacks.Count);
        for (int i = 0; i < _objectsPacks.Count; i++) {
            _objectsPacks[i].SetActive(rnd == i);
        }
    }

    private void OnExplored() {
        _interactableObject.RemoveFromPossibleCommands(Command.Search);
        //TODO drop resources around
        TriggerExplored();
    }

    private void TriggerExplored() {
        _animator.SetTrigger(Explored);
    }
}