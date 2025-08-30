using System;

public interface INotificationsService: IService {
    public Action<NotificationData> NewNotification { get;set; }
}
