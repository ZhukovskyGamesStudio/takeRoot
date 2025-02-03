using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementManager : MonoBehaviour {
    [Serializable]
    public enum MouseButton {
        RightButton,
        MiddleButton
    }

    [SerializeField]
    private bool _isEdgeMoving = true, _isDragMoving = true;

    [SerializeField]
    private float _cameraSpeed = 10f, _dragMultiplier = 2;

    [SerializeField]
    private float _edgeMargin = 20f;

    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private Rect _cameraBounds;

    [SerializeField]
    private float _zoomSpeed = 2f; // Zoom speed

    [SerializeField]
    private float _minZoom = 5f; // Minimum zoom level

    [SerializeField]
    private float _maxZoom = 20f; // Maximum zoom level

    [SerializeField]
    private MouseButton _mouseButtonToUse = MouseButton.RightButton;

    private Vector2 _currentPosition;
    private bool _isDragging;
    private Camera _main;

    private Vector2 _startPosition;

    void Start() {
        _main = Camera.main; // Cache Camera.main once in Start
    }

    void Update() {
        if (_isDragMoving) {
            TryDragCameraWithMouse();
        }

        if (!_isDragging && _isEdgeMoving) {
            TryMoveCameraNearScreenEdge();
        }

        HandleZoom();

        ClampCameraPositionAndZoom();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(_cameraBounds.center.x, _cameraBounds.center.y, 0),
            new Vector3(_cameraBounds.width, _cameraBounds.height, 0));
    }

    private void TryMoveCameraNearScreenEdge() {
        Vector3 cameraMovement = Vector3.zero;
        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x <= _edgeMargin) {
            cameraMovement.x = -1;
        } else if (mousePosition.x >= Screen.width - _edgeMargin) {
            cameraMovement.x = 1;
        }

        if (mousePosition.y <= _edgeMargin) {
            cameraMovement.y = -1;
        } else if (mousePosition.y >= Screen.height - _edgeMargin) {
            cameraMovement.y = 1;
        }

        if (cameraMovement != Vector3.zero) {
            _cameraTransform.position += cameraMovement * _cameraSpeed * Time.deltaTime;
        }
    }

    private void TryDragCameraWithMouse() {
        bool isPressed = _mouseButtonToUse == MouseButton.RightButton
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
        Vector2 delta = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector3 movement = new Vector3(delta.x, delta.y, 0);
        _cameraTransform.position += movement * _dragMultiplier * (_main.orthographicSize / _minZoom) * Time.deltaTime;
    }

    private void DragCamera() {
        Vector2 delta = _currentPosition - _startPosition;
        Vector3 movement = new Vector3(delta.x, delta.y, 0);
        _cameraTransform.position -= movement * _dragMultiplier * (_main.orthographicSize / _minZoom) * Time.deltaTime;

        _startPosition = _currentPosition;
    }

    private void ClampCameraPositionAndZoom() {
        _main.orthographicSize = Mathf.Clamp(_main.orthographicSize, _minZoom, _maxZoom); // Clamp zoom level to min/max
        float cameraWidth = _main.orthographicSize * 2 * _main.aspect;
        float cameraHeight = _main.orthographicSize * 2;

        float screenSeenPercent = cameraWidth / (_cameraBounds.xMax - _cameraBounds.xMin);
        if (screenSeenPercent > 1) {
            _main.orthographicSize /= screenSeenPercent;
            cameraWidth = _main.orthographicSize * 2 * _main.aspect;
            cameraHeight = _main.orthographicSize * 2;
        }

        float clampedX = Mathf.Clamp(_cameraTransform.position.x, _cameraBounds.xMin + cameraWidth / 2, _cameraBounds.xMax - cameraWidth / 2);
        float clampedY = Mathf.Clamp(_cameraTransform.position.y, _cameraBounds.yMin + cameraHeight / 2, _cameraBounds.yMax - cameraHeight / 2);
        _cameraTransform.position = new Vector3(clampedX, clampedY, _cameraTransform.position.z);
    }

    private void HandleZoom() {
        float scrollInput = Mouse.current.scroll.ReadValue().y; // Get the scroll wheel input

        if (scrollInput != 0) {
            // Adjust the camera's orthographic size based on the scroll input
            _main.orthographicSize -= scrollInput * _zoomSpeed;
            if (scrollInput > 0) {
                TryMoveCameraToZoom();
            }
        }
    }
}