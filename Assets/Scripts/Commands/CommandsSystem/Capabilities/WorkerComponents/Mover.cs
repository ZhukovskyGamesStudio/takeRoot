using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine.Serialization;

public class Mover : NetworkBehaviour, IMovable, IPathfinderUser {
	[FormerlySerializedAs("moveSpeed"), Header("Movement Settings")]
	public float moveTime = 1f;

	public float gridSize = 1f;
    
	[SerializeField]
	private Line _linePrefab;

	[SerializeField] 
	private GameObject _targetIcon;
	private GameObject _targetIconInstance;
    
	private Vector2 _targetPosition;
	private bool _isMoving;
	private Coroutine _moveCoroutine;
	private IPathfindService _pathfindService;
	private List<Vector2> _path = new List<Vector2>();
	private List<Line> _lineList = new List<Line>();
	private bool _linesShown;
	private Line _firstLine;

	private Vector2 position => new(transform.position.x, transform.position.y);
	private Vector2 positionInt => new(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
	public bool IsMoving => _isMoving;

	[SerializeField]
	private bool RotateWhileMove = true;

	private WorkerAnimator _workerAnimator;

	private void Start() {
		_pathfindService = ServiceLocator.Container.Single<IPathfindService>();
		_targetIconInstance = Instantiate(_targetIcon);
		_targetIconInstance.SetActive(false);
	}

	public void MoveTo(Vector2 targetPos) {
		if (_isMoving) {
			return;
		}

		if (_path == null || _targetPosition != targetPos) {
			_targetPosition = targetPos;
			_path = _pathfindService.FindPath(position, targetPos, this);
			UpdateLines();
			if (_linesShown)
				SwitchPathLine(true);
		}

		if (_path == null) {
			return;
		}

		int indexOfNextStep = _path.IndexOf(position) + 1;
		if (indexOfNextStep == _path.Count) {
			return;
		}

		Vector2 nextPos = _path[indexOfNextStep];

		if (_moveCoroutine != null) {
			return;
		}

		MoveToClientRpc(nextPos);
	}

	[ClientRpc]
	private void MoveToClientRpc(Vector2 nextPos) {
		_moveCoroutine = StartCoroutine(MoveToCell(nextPos, _workerAnimator));
	}

	public bool IsAtPosition(Vector2 target) {
		return position == target;
	}

	public bool HasPath(Vector2 target) {
		List<Vector2> path = _pathfindService.FindPath(position, target, this);
		return path != null;
	}

	public void SetMoveTime(float time) {
		moveTime = time;
	}
	
	public void SwitchPathLine(bool isOn) {
		if (isOn) {
			var enable = false;
			for (int i = 0; i < _path.Count - 1; i++) {
				if (_path[i] == positionInt) {
					enable = true;
					_firstLine = _lineList[i];
				}

				if (enable) {
					_lineList[i].gameObject.SetActive(true);
				}
			}

			if (_path.Count > 0 && _path.Last() != positionInt) {
				_targetIconInstance.transform.position = _path.Last();
				_targetIconInstance.SetActive(true);
			}
			else {
				_targetIconInstance.SetActive(false);
			}
		}
		else {
			_targetIconInstance.SetActive(false);
			foreach (Line line in _lineList) {
				line.gameObject.SetActive(false);
			}
		}
		_linesShown = isOn;
	}

	private void UpdateLines() {
		foreach (Line line in _lineList) {
			Destroy(line.gameObject);
		}
		_lineList.Clear();
		if (_path.Count == 0) return;
		for (int i = 0; i < _path.Count - 1; i++) {
			var line = Instantiate(_linePrefab);
			line.gameObject.SetActive(false);
			line.Init(_path[i], _path[i + 1]);
			_lineList.Add(line);
		}
	}

	private IEnumerator MoveToCell(Vector2 target, WorkerAnimator workerAnimator = null) {
		Vector3 target3 = new(target.x, target.y);
		Vector3 diff = target3 - transform.localPosition;
		
		RotateToMoveDirection(diff);
		workerAnimator?.PlayMove();
		yield return StartCoroutine(LerpFromTo(transform.localPosition, target3 * gridSize, moveTime, workerAnimator));
		workerAnimator?.ResetToIdle();
		yield return new WaitForSeconds(0.1f);
		_moveCoroutine = null;
	}

	private IEnumerator LerpFromTo(Vector3 from, Vector3 to, float time, WorkerAnimator workerAnimator = null) {
		float elapsedTime = 0f;
		_isMoving = true;

		while (elapsedTime < time) {
			float t = elapsedTime / time;
			t = Mathf.SmoothStep(0f, 1f, t);
			if (workerAnimator == null) {
				transform.localPosition = Vector3.Lerp(from, to, t);
				elapsedTime += Time.deltaTime;
			} else if (!workerAnimator.OnContactPointWhileMove) {
				transform.localPosition = Vector3.Lerp(from, to, t);
				elapsedTime += Time.deltaTime;
			}

			if (_firstLine != null)
				_firstLine.Init(transform.localPosition, to);
			yield return null;
		}

		transform.localPosition = to;
		if (_linesShown) {
			SwitchPathLine(true);
		}

		_isMoving = false;
	}

	private void RotateToMoveDirection(Vector3 diff) {
		if (!RotateWhileMove) {
			return;
		}

		if (diff.x < 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
		}

		if (diff.x > 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
	}

	private Vector2 GetGridPosition(Vector2 worldPosition) {
		return new Vector2(Mathf.Round(worldPosition.x / gridSize) * gridSize, Mathf.Round(worldPosition.y / gridSize) * gridSize);
	}

	public void Stop() {
		StopClientRpc();
	}

	[ClientRpc]
	private void StopClientRpc() {
		_isMoving = false;
		if (_moveCoroutine != null) {
			StopCoroutine(_moveCoroutine);
			_path = null;
			_moveCoroutine = null;
		}
	}

	private void OnDrawGizmos() {
		if (_path == null) {
			return;
		}

		foreach (Vector2 pos in _path) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(pos, new Vector3(gridSize, gridSize));
		}
	}

	public void Init(WorkerAnimator animator) {
		_workerAnimator = animator;
	}

	public void Cancel() {
		_workerAnimator.ResetToIdle();
	}
}