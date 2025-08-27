using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class InputService : IInputService {
    public Camera CameraMain => Camera.main != null ? Camera.main : null;

    public bool GetMouseButtonDown(MouseButton mouseButton) {
        return Input.GetMouseButtonDown((int)mouseButton) && !EventSystem.current.IsPointerOverGameObject();
    }

    public bool GetMouseButtonUp(MouseButton mouseButton) {
        return Input.GetMouseButtonUp((int)mouseButton) && !EventSystem.current.IsPointerOverGameObject();
    }

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
}