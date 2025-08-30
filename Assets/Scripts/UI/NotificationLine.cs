using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationLine : MonoBehaviour {
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<NotificationType, Color> Colors { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Button _deleteButton;

    private Action<NotificationData> _onDelete;
    private NotificationData _data;

    public void SetData(NotificationData data, Action<NotificationData> onDelete) {
        _data = data;
        _onDelete = onDelete;
        _text.text = data.Message;
        SetNotificationColor(data);
        _deleteButton.gameObject.SetActive(data.IsDeletable);
    }

    private void SetNotificationColor(NotificationData data) {
        try {
            _text.color = Colors[data.Type];
        } catch (Exception e) {
            Debug.LogError($"NotificationLine: Color for NotificationType {data.Type} not found. Exception: {e.Message}");
            _text.color = Color.white; // Fallback color
        }
    }

    public void Delete() {
        _onDelete?.Invoke(_data);
    }
}