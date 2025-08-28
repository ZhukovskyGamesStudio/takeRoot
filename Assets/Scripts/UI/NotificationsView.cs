using System.Collections.Generic;
using UnityEngine;

public class NotificationsView : MonoBehaviour {
    [SerializeField]
    private NotificationLine _linePrefab;

    [SerializeField]
    private Transform _linesContainer;

    private Dictionary<NotificationData, NotificationLine> _lines = new Dictionary<NotificationData, NotificationLine>();

    public void Init() {
        ClearLines();
    }

    public void AddLine(NotificationData data) {
        var line = Instantiate(_linePrefab, _linesContainer);
        line.SetData(data, OnDeleteLine);
        _lines.Add(data, line);
    }

    private void ClearLines() {
        foreach (Transform child in _linesContainer) {
            Destroy(child.gameObject);
        }
    }

    private void OnDeleteLine(NotificationData data) {
        if (_lines.Remove(data, out var line)) {
            Destroy(line.gameObject);
        }
    }
}