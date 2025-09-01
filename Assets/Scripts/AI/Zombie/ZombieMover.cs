using System;
using System.Collections.Generic;
using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AI {
	public class ZombieMover : MonoBehaviour, IZombieMover {
		public float moveTime = 1f;
		public float gridSize = 1f;

		private List<Vector2> _path;

		[SerializeField]
		private bool RotateWhileMove = true;

		private IPathfindService _pathfinder;
		private CancellationTokenSource _taskCts;

		private Vector2 position => new(transform.position.x, transform.position.y);
		public bool IsMoving { get; private set; }

		private void Start() {
			_pathfinder = ServiceLocator.Container.Single<IPathfindService>();
		}

		public bool IsAtPosition(Vector2 target) {
			return position == target;
		}

		public bool HasPath(Vector2 target) {
			return _pathfinder.FindPath(position, target).Count > 0;
		}

		public void MoveTo(Vector2 target) {
			if (IsMoving) return;
			if (_path == null) {
				_path = _pathfinder.FindPath(position, target);
			}
			
			int indexOfNextStep = _path.IndexOf(position) + 1;
			if (indexOfNextStep == _path.Count) {
				return;
			}

			Vector2 next = _path[indexOfNextStep];
			
			_taskCts = new CancellationTokenSource();
			DoMove(next, _taskCts.Token).Forget();
		}

		private async UniTaskVoid DoMove(Vector2 next, CancellationToken token) {
			IsMoving = true;
			var elapsedTime = 0f;
			while (!token.IsCancellationRequested && elapsedTime < moveTime) {
				float t = elapsedTime / moveTime;
				t = Mathf.SmoothStep(0f, 1f, t);
				transform.position = Vector3.Lerp(position, next, t);
				elapsedTime += Time.deltaTime;
				await UniTask.Yield();
			}
			transform.position = next;
			IsMoving = false;
		}
		
		private void OnDrawGizmos() {
			if (_path == null) {
				return;
			}

			foreach (Vector2 pos in _path) {
				Gizmos.color = Color.red;
				Gizmos.DrawWireCube(pos, new Vector3(gridSize, gridSize));
			}
		}
	}
}