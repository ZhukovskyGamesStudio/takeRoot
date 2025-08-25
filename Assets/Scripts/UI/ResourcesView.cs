using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ResourcesView : MonoBehaviour {
    [SerializeField]
    private int _maxVisibleResources = 10;

    [SerializeField]
    private Button _upButton, _downButton;

    [SerializeField]
    private ResourceLineView _resorceLinePrefab;

    [SerializeField]
    private Transform _resourcesContainer;

    private List<ResourceLineView> _resourceLines;

    private void InitEmptyLines() {
        _resourceLines = new List<ResourceLineView>();
        for (int i = 0; i < _maxVisibleResources; i++) {
            var line = Instantiate(_resorceLinePrefab, _resourcesContainer);
            _resourceLines.Add(line);
            line.gameObject.SetActive(false);
        }
    }

    public void SetData(Dictionary<ResourceType, int> dictionary) {
        if (_resourceLines == null) {
            InitEmptyLines();
        }

        var isTooLong = dictionary.Count >= _maxVisibleResources;
        _upButton.gameObject.SetActive(isTooLong);
        _downButton.gameObject.SetActive(isTooLong);
        foreach (var line in _resourceLines!) {
            line.gameObject.SetActive(false);
        }

        int curShown = Mathf.Min(dictionary.Count, _maxVisibleResources);

        for (int i = 0; i < curShown; i++) {
            var line = _resourceLines[i];
            var type = dictionary.Keys.ElementAt(i);
            var amount = dictionary.Values.ElementAt(i);
            line.gameObject.SetActive(true);
            line.SetData(type, amount);
        }
    }

    //TODO add working up and down buttons
}