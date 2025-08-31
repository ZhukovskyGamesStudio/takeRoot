using System;
using System.Collections.Generic;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AI {
	public class ZombieMover : MonoBehaviour, IZombieMover {
		public float moveTime = 1f;
		public float gridSize = 1f;
		
		[SerializeField]
		private bool RotateWhileMove = true;

		private IPathfindService _pathfinder;
		private List<Vector2> _path;
		private Vector2 position => new(transform.position.x, transform.position.y);
		public bool IsMoving { get; private set; }

		private void Start() {
			_pathfinder = ServiceLocator.Container.Single<IPathfindService>();
		}

		public bool IsAtPosition(Vector2 target) {
			return false;
		}

		public bool HasPath(Vector2 target) {
			return true;
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
			
			
		}

		private async UniTaskVoid DoMove(Vector2 next) {
			IsMoving = true;
			
			
		}
	}
}