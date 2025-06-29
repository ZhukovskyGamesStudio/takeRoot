using UnityEngine;

public interface IMovable {
	bool TryMoveTo(Vector2 position);
	bool IsAtPosition(Vector2 position);
	Vector2 Position { get; }
	bool IsMoving { get; }
	void Stop();
} 