using System;
using CodeBase.Services;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedView : NetworkBehaviour {
    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<GameSpeedType, Toggle> _speedToggles;

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<GameSpeedType, GameObject> _friendSelection;

    private Action<GameSpeedType, Race> _onSpeedSelect;

    public void Init(Action<GameSpeedType, Race> onSpeedSelect, ReactiveProperty<bool> isReadyToPause) {
        _onSpeedSelect = onSpeedSelect;
        InitToggles();
        isReadyToPause.Subscribe(ChangeSpeedAvailable);
    }

    private void InitToggles() {
        foreach (var kvp in _speedToggles) {
            kvp.Value.onValueChanged.AddListener(isOn => {
                if (isOn) {
                    SelectSpeed(kvp.Key);
                }
            });
        }
    }

    private void ChangeSpeedAvailable(bool isOn) {
        _speedToggles[GameSpeedType.Paused].interactable = isOn;
    }

    private void SelectSpeed(GameSpeedType type) {
        SelectSpeedServerRpc(type, ServiceLocator.Container.Single<INetworkService>().MyRace.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectSpeedServerRpc(GameSpeedType speedType, Race race) {
        _onSpeedSelect?.Invoke(speedType, race);
        Debug.Log("Selected speed: " + speedType);
    }

    [ClientRpc]
    public void SetFriendSelectionClientRpc(GameSpeedType type) {
        foreach (var kvp in _friendSelection) {
            kvp.Value.SetActive(kvp.Key == type);
        }
    }
}