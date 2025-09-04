using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarsView : MonoBehaviour {
    [SerializeField]
    private Transform _settlersContainer;

    [SerializeField]
    private AvatarView _settlerViewPrefab;
    [SerializeField]
    private ToggleGroup _toggleGroup;
    public void InitSettlers(IEnumerable<AI.Settler> settlers, Action<SettlerSelectable> onSelectSettler) {
        foreach (Transform child in _settlersContainer) {
            Destroy(child.gameObject);
        }

        foreach (AI.Settler settler in settlers) {
            AvatarView newSettler = Instantiate(_settlerViewPrefab, _settlersContainer);

            newSettler.Init(settler,onSelectSettler,_toggleGroup);
        }
    }

    public void UpdateSettlers() {
        foreach (Transform child in _settlersContainer) {
            child.GetComponent<AvatarView>().UpdateData();
        }
    }
}