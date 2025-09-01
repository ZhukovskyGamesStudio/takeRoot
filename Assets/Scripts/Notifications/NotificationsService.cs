using System;

public class NotificationsService : INotificationsService, IUpdatable {
    private readonly IUpdateService _updateService;

    //private DateTime _lastNotificationTime;
    private float _secondsBetweenNotifications = 15f;

    public Action<NotificationData> NewNotification { get; set; }

    public NotificationsService(IUpdateService updateService, IOccurenceService occurenceService) {
        _updateService = updateService;
        _updateService.Register(this);
        //_lastNotificationTime = DateTime.Now - TimeSpan.FromHours(1);
        occurenceService.OnOccurenceSpawn += ShowOccurenceNotification;
    }

    private void ShowOccurenceNotification(OccurenceConfig occurenceConfig) {
        NewNotification?.Invoke(new NotificationData()
            { Type = NotificationType.Alarm, Message = $"A new {occurenceConfig.Type} has coming!", IsDeletable = false });
    }

    public void Update() {
       /* if ((DateTime.Now - _lastNotificationTime).TotalSeconds >= _secondsBetweenNotifications) {
            NewNotification?.Invoke(new NotificationData() {
                Type = UnityEngine.Random.Range(0, 3) switch {
                    0 => NotificationType.Info,
                    1 => NotificationType.Warning,
                    2 => NotificationType.Alarm
                },
                Message = "This is a random notification",
                IsDeletable = true
            });
            _lastNotificationTime = DateTime.Now;
        }*/
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }
}