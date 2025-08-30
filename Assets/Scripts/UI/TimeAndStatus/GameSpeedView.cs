using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedView : MonoBehaviour {
    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<GameSpeedType, Toggle> _speedToggles;

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<GameSpeedType, GameObject> _friendSelection;

    private Action<GameSpeedType> _onSpeedSelect;

    public void Init(Action<GameSpeedType> onSpeedSelect) {
        _onSpeedSelect = onSpeedSelect;
        InitToggles();
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

    private void SelectSpeed(GameSpeedType type) {
        _onSpeedSelect?.Invoke(type);
    }

    public void SetFriendSelection(GameSpeedType type) {
        foreach (var kvp in _friendSelection) {
            kvp.Value.SetActive(kvp.Key == type);
        }
    }
}