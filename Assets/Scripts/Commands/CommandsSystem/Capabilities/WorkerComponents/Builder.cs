using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AI {
	public class Builder : MonoBehaviour ,IBuilder {
		public float buildTime;
		private bool _isBuilding;
		
		private WorkerAnimator _animator;
		private IAsyncRunner _asyncRunner;
		private CancellationTokenSource _taskCts;

		public void Init(WorkerAnimator animator) {
			_animator = animator;
			_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
		}

		public void Build(BuildingBlueprint buildingBlueprint) {
			if (_isBuilding) {
				return;
			}
			
			_taskCts = new CancellationTokenSource();
			DoBuild(buildingBlueprint, _taskCts.Token).Forget();
		}

		private async UniTaskVoid DoBuild(BuildingBlueprint buildingBlueprint, CancellationToken token) {
			_isBuilding = true;
			_animator.PlayHit();
			while (!token.IsCancellationRequested) {
				await _asyncRunner.Wait(buildTime, token);
				if (token.IsCancellationRequested) {
					break;
				}

				if (buildingBlueprint.WasBuilded) break;
				buildingBlueprint.Build();
			}
		}
		
		public void Cancel() {
			if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
				_taskCts.Cancel();
				_taskCts.Dispose();
				_taskCts = null;
				_animator.ResetToIdle();
				_isBuilding = false;
			}
		}
	}
}