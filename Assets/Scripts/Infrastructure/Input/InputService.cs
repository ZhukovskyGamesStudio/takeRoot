using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class InputService : IInputService, IUpdatable, IDisposable {
    private readonly IUpdateService _updateService;
    public Camera CameraMain => Camera.main != null ? Camera.main : null;

    private bool _isSelectionStarted;
    private Vector2 _startPos;

    public InputService(IUpdateService updateService) {
        _updateService = updateService;
        _updateService.Register(this);
    }

    public bool GetMouseButtonDown(MouseButton mouseButton) {
        bool res = Input.GetMouseButtonDown((int)mouseButton) && !EventSystem.current.IsPointerOverGameObject();
        if (res) {
            _isSelectionStarted = true;
            _startPos = GetWorldMousePosition();
        }

        return res;
    }

    public bool GetMouseButtonUp(MouseButton mouseButton) {
        bool res = Input.GetMouseButtonUp((int)mouseButton) && !EventSystem.current.IsPointerOverGameObject();
        return res;
    }

    public Action<Rect> OnSelectionEnd { get; set; }

    public Vector2 GetScreenMousePosition() {
        return CameraMain ? (Vector2)UnityEngine.Input.mousePosition : Vector2.zero;
    }

    public Vector2 GetWorldMousePosition() {
        if (CameraMain == null) {
            return Vector2.zero;
        }

        Vector3 screenPosition = new(Input.mousePosition.x, Input.mousePosition.y, CameraMain.nearClipPlane);
        Vector3 worldPos = CameraMain.ScreenToWorldPoint(screenPosition);

        return new Vector2(worldPos.x, worldPos.y);
    }

    public bool GetKeyDown(KeyCode keyCode) {
        return Input.GetKeyDown(keyCode);
    }

    public void Update() {
        if (GetMouseButtonDown(MouseButton.Left)) {
            _isSelectionStarted = true;
        }

        if (_isSelectionStarted && Input.GetMouseButtonUp((int)MouseButton.Left)) {
            _isSelectionStarted = false;
            var endPos = GetWorldMousePosition();
            var resRect = new Rect(Mathf.Min(_startPos.x, endPos.x), Mathf.Min(_startPos.y, endPos.y), Mathf.Abs(_startPos.x - endPos.x),
                Mathf.Abs(_startPos.y - endPos.y));
            OnSelectionEnd?.Invoke(resRect);
        }
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }
}