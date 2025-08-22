using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CoreCanvasUi : NetworkBehaviour, IInitableInstance {
    [SerializeField]
    private InfoBookView _infoPanel;

    [SerializeField]
    private SettlerPanel _settlerPanel;

    [SerializeField]
    private Toggle _infoToggle;

    [SerializeField]
    private Transform _settlersContainer;

    [SerializeField]
    private SettlerView _settlerViewPrefab;

    private void FixedUpdate() {
        UpdateSettlers();
    }

    public void Init() {
        ObsoleteCoreEntryPoint.UI = this;
        InitRace();
        InitSettlers(ObsoleteCoreEntryPoint.SettlersManager.MySettlers);
    }

    public void InitSettlers(IEnumerable<SettlerData> settlers) {
        foreach (Transform child in _settlersContainer) {
            Destroy(child.gameObject);
        }

        foreach (SettlerData settler in settlers) {
            SettlerView newSettler = Instantiate(_settlerViewPrefab, _settlersContainer);
            
            newSettler.Init(settler);
        }
    }

    private void UpdateSettlers() {
        foreach (Transform child in _settlersContainer) {
            child.GetComponent<SettlerView>().UpdateData();
        }
    }

    private void InitRace() {
        if (NetworkManager.Singleton != null) {
            SetRace(PlayerRaceSelection.GetRace());
        } else {
            SetRace(ObsoleteCoreEntryPoint.Instance.CurrentNetworkFakeRace);
        }
    }

    private void SetRace(Race race) {
        IHasRaceVariant[] variableChildren = transform.GetComponentsInChildren<IHasRaceVariant>();
        foreach (IHasRaceVariant variable in variableChildren) {
            variable.SetVariant(race);
        }
    }

    public void OpenInfoPanel(ISelectable selectable) {
        CloseInfoPanel();
        _infoToggle.isOn = true;

        if (selectable.GetGameObject().TryGetComponent(out CraftingStationable craftingStationable)) {
            _infoPanel.Init(craftingStationable);
            return;
        }

        _infoPanel.Init(selectable.GetInfoData());
    }

    public void OpenSettlerPanel(AI.SettlerData settlerData) {
        CloseInfoPanel();

        _settlerPanel.gameObject.SetActive(true);
        _settlerPanel.SetData(settlerData);
    }

    public void CloseInfoPanel() {
        _settlerPanel.gameObject.SetActive(false);
        _infoPanel.gameObject.SetActive(false);
        _infoToggle.isOn = false;
    }
}