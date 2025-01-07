using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomView : MonoBehaviour {
    [SerializeField]
    private bool _isSelectingRandomViewVariant;

    [SerializeField]
    private List<GameObject> _objectsPacks;

    private void Start() {
        if (_isSelectingRandomViewVariant) {
            SelectRandomViewVariant();
        }
    }

    public void SelectRandomViewVariant() {
        int rnd = Random.Range(0, _objectsPacks.Count);
        for (int i = 0; i < _objectsPacks.Count; i++) {
            _objectsPacks[i].SetActive(rnd == i);
        }
    }
}