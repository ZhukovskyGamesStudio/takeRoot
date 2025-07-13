using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine.Serialization;

public class Mover : MonoBehaviour, IMovable {
	[FormerlySerializedAs("moveSpeed")] [Header("Movement Settings")]
	public float moveTime = 1f;
	public float gridSize = 1f;
	
	private Vector2 _targetPosition;
	private bool _isMoving;
	private Coroutine _moveCoroutine;
	private IPathfindService _pathfindService;
	private List<Vector2> _path;
	
	private Vector2 position => new Vector2(transform.position.x, transform.position.y);
	public bool IsMoving => _isMoving;
	[SerializeField] private bool RotateWhileMove = true;
	
	private void Start() {
		_pathfindService = ServiceLocator.Container.Single<IPathfindService>();
	}
	
	public void MoveTo(Vector2 targetPos, WorkerAnimator workerAnimator = null) {
		if (_path == null || _targetPosition != targetPos) {
			_targetPosition = targetPos;
			_path = _pathfindService.FindPath(position, targetPos);
		}
		if (_path == null) return; //TODO: evaluate path
		
		_isMoving = true;
		var indexOfNextStep = _path.IndexOf(position) + 1;
		if (indexOfNextStep == _path.Count) return;
		var next = _path[indexOfNextStep];

		if (_moveCoroutine != null) return;
		
		_moveCoroutine = StartCoroutine(MoveToCell(next, workerAnimator));
		return;
	}
	
	public bool IsAtPosition(Vector2 target) {
		return position == target;
	}

	public bool HasPath(Vector2 target) {
		var path = _pathfindService.FindPath(position, target);
		return path != null;
	}

	public void SetMoveTime(float time) {
		moveTime = time;
	}

	private IEnumerator MoveToCell(Vector2 target, WorkerAnimator workerAnimator = null)
	{
		Vector3 target3 = new Vector3(target.x, target.y);
		Vector3 diff = target3 - transform.localPosition;
     
		RotateToMoveDirection(diff);
		workerAnimator?.PlayMove();
		yield return StartCoroutine(LerpFromTo(transform.localPosition, target3 * gridSize, moveTime));
		workerAnimator?.ResetToIdle();
		_moveCoroutine = null;
	}

	private IEnumerator LerpFromTo(Vector3 from, Vector3 to, float time) {
		float elapsedTime = 0f;
		_isMoving = true;

		while (elapsedTime < time) {
			float t = elapsedTime / time;
			t = Mathf.SmoothStep(0f, 1f, t);
			transform.localPosition = Vector3.Lerp(from, to, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = to;
		_isMoving = false;
	}

	private void RotateToMoveDirection(Vector3 diff) {
		if (!RotateWhileMove) return;
		if (diff.x < 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
		}

		if (diff.x > 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
	}

	private Vector2 GetGridPosition(Vector2 worldPosition) {
		return new Vector2(
			Mathf.Round(worldPosition.x / gridSize) * gridSize,
			Mathf.Round(worldPosition.y / gridSize) * gridSize
		);
	}

	public void Stop() {
		_isMoving = false;
		if (_moveCoroutine != null) {
			StopCoroutine(_moveCoroutine);
			_path = null;
			_moveCoroutine = null;
		}
	}

	private void OnDrawGizmos() {
		if (_path == null) return;

		foreach (Vector2 pos in _path) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(pos, new Vector3(gridSize, gridSize));
		}
	}
} 