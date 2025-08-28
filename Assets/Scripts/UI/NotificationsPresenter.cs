public class NotificationsPresenter {
    private readonly NotificationsView _view;
    private readonly INotificationsService _notificationsService;

    public NotificationsPresenter(NotificationsView view, INotificationsService notificationsService) {
        _view = view;
        _notificationsService = notificationsService;
        _view.Init();
        _notificationsService.NewNotification += view.AddLine;
    }
}