using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsPanelView : NetworkBehaviour {
    [SerializeField]
    private BuildingPanelGridView _itemPrefab;

    [SerializeField]
    private Transform _gridItemsContainer;

    [SerializeField]
    private TextMeshProUGUI _categoryHeader;
    [SerializeField]
    private int _shownAmount = 9;

    [SerializeField]
    private ToggleGroup _toggleGroup;

    [SerializeField]
    private BuildingsPanelInfoPage _infoPage;

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<BuildingCategory, Toggle> _categoryToggles;

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<BuildingCategory, string> _categoryNames;
    
    [Header("Remove from here!!!"), SerializeField]
    private List<BuildingRecipeConfig> _mockRecipeConfigs;

    private List<BuildingPanelGridView> _gridItems;
    private List<BuildingRecipeConfig> _recipeConfigs;
    private BuildingRecipeConfig _selectedConfig;
    private Action<string, Race> _onBuild;

    private BuildingCategory _currentCategory = BuildingCategory.General;
    private IResearchService _researchService;

    private void InitToggles() {
        foreach (KeyValuePair<BuildingCategory, Toggle> kvp in _categoryToggles) {
            kvp.Value.onValueChanged.AddListener(isOn => {
                if (isOn) {
                    _currentCategory = kvp.Key;
                    UpdateCategory();
                }
            });
        }
    }

    private void OnEnable() {
        _infoPage.SetEmptyData();
    }

    protected override void OnNetworkPostSpawn() {
        base.OnNetworkPostSpawn();
        InitToggles();
        gameObject.SetActive(false);
    }

    private void CreateEmptyGrid() {
        _gridItems = new List<BuildingPanelGridView>();
        for (int i = 0; i < _shownAmount; i++) {
            BuildingPanelGridView item = Instantiate(_itemPrefab, _gridItemsContainer);
            item.Init(OpenInfoPanel, _toggleGroup);
            _gridItems.Add(item);
        }
    }

    public void SetData(List<BuildingRecipeConfig> costConfigs, Action<string, Race> onBuild) {
        _onBuild = onBuild;
        if (_gridItems == null) {
            CreateEmptyGrid();
        }

        _researchService = ServiceLocator.Container.Single<IResearchService>();
        _researchService.OnResearchFinished += UpdateCategory;
        _recipeConfigs = costConfigs;
        UpdateCategory();
    }

    private void UpdateCategory() {
        foreach (BuildingPanelGridView item in _gridItems) {
            item.gameObject.SetActive(false);
        }

        List<BuildingRecipeConfig> curShown;
        if (AdminManager.IsHumanBuildingsBuildable)
            curShown = _recipeConfigs.Where(c => c.BuildingCategory == _currentCategory && (c.RequiredResearch == Research.None || _researchService.WasResearched(c.RequiredResearch)) && c.IsHumanBuilding).ToList();
        else
            curShown = _recipeConfigs.Where(c => c.BuildingCategory == _currentCategory && (c.RequiredResearch == Research.None || _researchService.WasResearched(c.RequiredResearch))).ToList();

        for (int i = 0; i < curShown.Count; i++) {
            _gridItems[i].gameObject.SetActive(true);
            _gridItems[i].SetData(curShown[i]);
        }

        _categoryHeader.text = _categoryNames[_currentCategory];
    }

    private void OpenInfoPanel(BuildingRecipeConfig config) {
        _selectedConfig = config;
        _infoPage.SetData(config);
    }

    public void Build() {
        Debug.Log("Build " + _selectedConfig.mainInfo.Name);
        BuildServerRpc(_selectedConfig.mainInfo.Name, ServiceLocator.Container.Single<INetworkService>().MyRace.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void BuildServerRpc(string buildingName, Race race) {
        Debug.Log("BuildServerRpc " + buildingName);
        _onBuild?.Invoke(buildingName, race);
    }
}