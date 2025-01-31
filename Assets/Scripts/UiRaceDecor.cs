using UnityEngine;

public class UiRaceDecor : MonoBehaviour, IHasRaceVariant {
    [SerializeField]
    private Transform _holder;

    [SerializeField]
    private GameObject _plantsDecor, _robotsDecor;

    private GameObject _current;

    public void SetVariant(Race race) {
        if (_current != null) {
            Destroy(_current);
        } else {
            if (_holder.childCount > 0) {
                Destroy(_holder.GetChild(0).gameObject);
            }
        }

        _current = Instantiate(race == Race.Plants ? _plantsDecor : _robotsDecor, _holder);
    }
}