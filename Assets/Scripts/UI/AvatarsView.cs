using System.Collections.Generic;
using UnityEngine;

public class AvatarsView : MonoBehaviour {
    [SerializeField]
    private Transform _settlersContainer;

    [SerializeField]
    private SettlerView _settlerViewPrefab;

    public void InitSettlers(IEnumerable<AI.Settler> settlers) {
        foreach (Transform child in _settlersContainer) {
            Destroy(child.gameObject);
        }

        foreach (AI.Settler settler in settlers) {
            SettlerView newSettler = Instantiate(_settlerViewPrefab, _settlersContainer);

            newSettler.Init(settler.Data);
        }
    }

    public void UpdateSettlers() {
        foreach (Transform child in _settlersContainer) {
            child.GetComponent<SettlerView>().UpdateData();
        }
    }
}