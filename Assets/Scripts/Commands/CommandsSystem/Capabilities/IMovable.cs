using UnityEngine;

public interface IMovable {
	void MoveTo(Vector2 position, WorkerAnimator workerAnimator);
	bool IsAtPosition(Vector2 target);
	bool HasPath(Vector2 target);
	void SetMoveTime(float time);
	void Stop();
} 