using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationLine : MonoBehaviour {
    [field: SerializeField]
    public AYellowpaper.SerializedCollections.SerializedDictionary<NotificationType, Color> Colors { get; private set; } = new();

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
        _text.color = Colors[data.Type];
        _deleteButton.gameObject.SetActive(data.IsDeletable);
    }

    public void Delete() {
        _onDelete?.Invoke(_data);
    }
}