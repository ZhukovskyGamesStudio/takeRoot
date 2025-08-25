using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsPanelView : MonoBehaviour {
    [SerializeField]
    private BuildingPanelGridView _itemPrefab;

    [SerializeField]
    private Transform _gridItemsContainer;

    [SerializeField]
    private int _shownAmount = 9;

    [SerializeField]
    private List<BuildingPanelGridView> _gridItems;

    private void Start() {
        SetData();
    }

    private void CreateEmptyGrid() {
        _gridItems = new List<BuildingPanelGridView>();
        for (int i = 0; i < _shownAmount; i++) {
            var item = Instantiate(_itemPrefab, _gridItemsContainer);
            item.SetData();
            _gridItems.Add(item);
        }
    }

    public void SetData() {
        if (_gridItems == null) {
            CreateEmptyGrid();
        }
        
        
        
        
    }
}