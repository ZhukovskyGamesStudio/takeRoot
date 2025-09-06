using UnityEngine;

public interface IMovable : IPerformerComponent {
    bool IsMoving { get; }
    void MoveTo(Vector2 position);
    bool IsAtPosition(Vector2 target);
    bool HasPath(Vector2 target);
    void SetMoveTime(float time);
    void SwitchPathLine(bool isOn);
    void Stop();
}