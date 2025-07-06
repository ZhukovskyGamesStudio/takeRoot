using UnityEngine;

public interface IMovable {
	bool TryMoveTo(Vector2 position);
	bool IsAtPosition(Vector2 target);
	void SetMoveTime(float time);
	void Stop();
} 