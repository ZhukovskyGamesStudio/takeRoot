using System;

[Serializable]
public class NotificationData {
    public NotificationType Type;
    public string Message;
    public bool IsDeletable;
}