using System;
using UniRx;

public class OverlaysPresenter : IDisposable {
    private readonly OverlaysView _view;
    private readonly IOverlayService _overlayService;
    public ReactiveProperty<OverlayType> CurrentOverlay { get; private set; } = new ReactiveProperty<OverlayType>(OverlayType.None);

    public OverlaysPresenter(OverlaysView view, IOverlayService overlayService) {
        _view = view;
        _overlayService = overlayService;
        foreach (var kvp in _view.Toggles) {
            kvp.Value.isOn = false;
            kvp.Value.onValueChanged.AddListener(isOn => {
                if (isOn) {
                    CurrentOverlay.Value = kvp.Key;
                    _overlayService.ChangeOverlayTo(kvp.Key);
                } else {
                    if (CurrentOverlay.Value == kvp.Key) {
                        CurrentOverlay.Value = OverlayType.None;
                        _overlayService.ChangeOverlayTo(kvp.Key);
                    }
                }
            });
        }
    }

    public void Dispose() {
        CurrentOverlay?.Dispose();
    }
}