using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "CameraMovementConfig", menuName = "Scriptable Objects/CameraMovementConfig", order = 0)]
public class CameraMovementConfig : ScriptableObject {
    public bool IsEdgeMoving = true, IsDragMoving = true;

    public float CameraSpeed = 10f, DragMultiplier = 2;

    public float EdgeMargin = 20f;

    public Rect CameraBounds;

    public float ZoomSpeed = 2f; // Zoom speed

    public float MinZoom = 5f; // Minimum zoom level

    public float MaxZoom = 20f; // Maximum zoom level

    public MouseButton MouseButtonToUse = MouseButton.Right;
}