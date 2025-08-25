using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsPanelView : MonoBehaviour {
    [SerializeField]
    private BuildingPanelGridView _itemPrefab;

    [SerializeField]
    private Transform _gridItemsContainer;

    [SerializeField]
    private int _shownAmount = 9;

    [SerializeField]
    private ToggleGroup _toggleGroup;

    [SerializeField]
    private BuildingsPanelInfoPage _infoPage;

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<BuildingCategory, Toggle> _categoryToggles;

    [Header("Remove from here!!!")]
    [SerializeField]
    private List<BuildingRecipeConfig> _mockRecipeConfigs;

    private List<BuildingPanelGridView> _gridItems;
    private List<BuildingRecipeConfig> _recipeConfigs;
    private BuildingRecipeConfig _selectedConfig;
    private Action<BuildingRecipeConfig> _onBuild;

    private BuildingCategory _currentCategory = BuildingCategory.General;

    private void InitToggles() {
        foreach (var kvp in _categoryToggles) {
            kvp.Value.onValueChanged.AddListener(isOn => {
                if (isOn) {
                    _currentCategory = kvp.Key;
                    UpdateCategory();
                }
            });
        }
    }

    private void Start() {
        InitToggles();
        SetData(_mockRecipeConfigs, recipeConfig => Debug.Log($"Starting build of {recipeConfig.HeaderName}"));
    }

    private void OnEnable() {
        _infoPage.SetEmptyData();
    }

    private void CreateEmptyGrid() {
        _gridItems = new List<BuildingPanelGridView>();
        for (int i = 0; i < _shownAmount; i++) {
            var item = Instantiate(_itemPrefab, _gridItemsContainer);
            item.Init(OpenInfoPanel, _toggleGroup);
            _gridItems.Add(item);
        }
    }

    public void SetData(List<BuildingRecipeConfig> costConfigs, Action<BuildingRecipeConfig> onBuild) {
        _onBuild = onBuild;
        if (_gridItems == null) {
            CreateEmptyGrid();
        }

        _recipeConfigs = costConfigs;
        UpdateCategory();
    }

    private void UpdateCategory() {
        foreach (var item in _gridItems) {
            item.gameObject.SetActive(false);
        }

        var curShown = _recipeConfigs.Where(c => c.BuildingCategory == _currentCategory).ToList();

        for (int i = 0; i < curShown.Count; i++) {
            _gridItems[i].gameObject.SetActive(true);
            _gridItems[i].SetData(curShown[i]);
        }
    }

    private void OpenInfoPanel(BuildingRecipeConfig config) {
        _selectedConfig = config;
        _infoPage.SetData(config);
    }

    public void Build() {
        _onBuild?.Invoke(_selectedConfig);
    }
}