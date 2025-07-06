using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VideoMoveTo : MonoBehaviour, IMovable {

	public float moveTime = 1f;
	public float gridSize = 1f;
	
	[SerializeField]public List<VideoPathsPositions> Paths;
	private Vector2 _targetPosition;
	private Coroutine _moveCoroutine;
	private VideoPathsPositions _path;
	private bool _isMoving;
	
	private Vector2 position => new Vector2((int)transform.position.x, (int)transform.position.y);
	
	public bool TryMoveTo(Vector2 position) {
		if (_targetPosition != position) {
			_path = Paths.First(c => (Vector2)c.positions[^1].position == position);
			_targetPosition = position;
		}
		
		if (_isMoving) return true;
		_isMoving = true;
		var indexOfNextStep = _path.positions.IndexOf(_path.positions.First(c => (Vector2)c.position == this.position)) + 1;
		if (indexOfNextStep == _path.positions.Count) return true;
		var next = (Vector2)(_path.positions[indexOfNextStep].position);

		if (_moveCoroutine != null) return true;
		
		_moveCoroutine = StartCoroutine(MoveToCell(next));
		return true;
	}

	public bool IsAtPosition(Vector2 target) {
		return position == target;
	}

	public void SetMoveTime(float time) {
		throw new System.NotImplementedException();
	}

	private IEnumerator MoveToCell(Vector2 target)
	{
		Vector3 target3 = new Vector3(target.x, target.y);
		Vector3 diff = target3 - transform.localPosition;
     
		RotateToMoveDirection(diff);
		yield return StartCoroutine(LerpFromTo(transform.localPosition, target3 * gridSize, moveTime));
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
}