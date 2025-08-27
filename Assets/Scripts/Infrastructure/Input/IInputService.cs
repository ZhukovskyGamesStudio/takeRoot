using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public interface IInputService : IService {
    public bool GetMouseButtonDown(MouseButton mouseButton);

    public bool GetMouseButtonUp(MouseButton mouseButton);

    public Vector2 GetScreenMousePosition();
    public Vector2 GetWorldMousePosition();
    public bool GetKeyDown(KeyCode keyCode);
}