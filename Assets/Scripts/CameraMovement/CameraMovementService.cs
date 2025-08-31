using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CameraMovementService : ICameraMovementService, IUpdatable, IDisposable {
    private readonly CameraMovementConfig _config;

    private Transform _cameraTransform;

    private Vector2 _currentPosition;
    private bool _isDragging;
    private Camera _main;

    private Vector2 _startPosition;
    private readonly IUpdateService _updateService;

    public CameraMovementService(IConfigsProvider configProvider, IUpdateService updateService) {
        _config = configProvider.CameraMovementConfig;
        _updateService = updateService;
        _updateService.Register(this);
    }

    private void FindMainCamera() {
        _main = Camera.main;
        _cameraTransform = _main!.transform;
    }

    public void Update() {
        if (_main == null) {
            FindMainCamera();
        }

        if (_config.IsDragMoving) {
            TryDragCameraWithMouse();
        }

        if (!_isDragging && _config.IsEdgeMoving) {
            TryMoveCameraNearScreenEdge();
        }

        HandleZoom();

        ClampCameraPositionAndZoom();
    }

    private void OnDrawGizmos() {
        if (_config == null) {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(_config.CameraBounds.center.x, _config.CameraBounds.center.y, 0),
            new Vector3(_config.CameraBounds.width, _config.CameraBounds.height, 0));
    }

    private void TryMoveCameraNearScreenEdge() {
        Vector3 cameraMovement = Vector3.zero;
        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x <= _config.EdgeMargin) {
            cameraMovement.x = -1;
        } else if (mousePosition.x >= Screen.width - _config.EdgeMargin) {
            cameraMovement.x = 1;
        }

        if (mousePosition.y <= _config.EdgeMargin) {
            cameraMovement.y = -1;
        } else if (mousePosition.y >= Screen.height - _config.EdgeMargin) {
            cameraMovement.y = 1;
        }

        if (cameraMovement != Vector3.zero) {
            _cameraTransform.position += cameraMovement * _config.CameraSpeed * Time.deltaTime;
        }
    }

    private void TryDragCameraWithMouse() {
        bool isPressed = _config.MouseButtonToUse == MouseButton.Right
            ? Mouse.current.rightButton.isPressed
            : Mouse.current.middleButton.isPressed;

        if (isPressed) {
            if (!_isDragging) {
                _startPosition = Mouse.current.position.ReadValue();
                _isDragging = true;
            }

            _currentPosition = Mouse.current.position.ReadValue();
            DragCamera();
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame || Mouse.current.middleButton.wasReleasedThisFrame) {
            _isDragging = false;
        }
    }

    private void TryMoveCameraToZoom() {
        Vector3 current = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 start = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
        Vector3 movement = current - start;
        movement.z = 0;

        _cameraTransform.position += movement;
    }

    private void DragCamera() {
        Vector3 current = Camera.main.ScreenToWorldPoint(_currentPosition);
        Vector3 start = Camera.main.ScreenToWorldPoint(_startPosition);
        Vector3 movement = current - start;
        movement.z = 0;

        _cameraTransform.position -= movement;

        _startPosition = _currentPosition;
    }

    private void ClampCameraPositionAndZoom() {
        _main.orthographicSize = Mathf.Clamp(_main.orthographicSize, _config.MinZoom, _config.MaxZoom); // Clamp zoom level to min/max
        float cameraWidth = _main.orthographicSize * 2 * _main.aspect;
        float cameraHeight = _main.orthographicSize * 2;

        float screenSeenPercent = cameraWidth / (_config.CameraBounds.xMax - _config.CameraBounds.xMin);
        if (screenSeenPercent > 1) {
            _main.orthographicSize /= screenSeenPercent;
            cameraWidth = _main.orthographicSize * 2 * _main.aspect;
            cameraHeight = _main.orthographicSize * 2;
        }

        float clampedX = Mathf.Clamp(_cameraTransform.position.x, _config.CameraBounds.xMin + cameraWidth / 2,
            _config.CameraBounds.xMax - cameraWidth / 2);
        float clampedY = Mathf.Clamp(_cameraTransform.position.y, _config.CameraBounds.yMin + cameraHeight / 2,
            _config.CameraBounds.yMax - cameraHeight / 2);
        _cameraTransform.position = new Vector3(clampedX, clampedY, _cameraTransform.position.z);
    }

    private void HandleZoom() {
        float scrollInput = Mouse.current.scroll.ReadValue().y; // Get the scroll wheel input

        if (scrollInput != 0) {
            // Adjust the camera's orthographic size based on the scroll input
            _main.orthographicSize -= scrollInput * _config.ZoomSpeed;
            if (scrollInput > 0) {
                TryMoveCameraToZoom();
            }
        }
    }

    public void Dispose() {
        _updateService.Unregister(this);
    }
}