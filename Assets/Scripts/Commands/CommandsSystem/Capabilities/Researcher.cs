using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AI {
	public class Researcher : MonoBehaviour, IResearcher {
		[Header("Researcher Settings")]
		public float researchTime = 1.5f;
		private float _lastResearchTime;
		private bool _isResearching;

		private WorkerAnimator _animator;
		private IAsyncRunner _asyncRunner;
		private CancellationTokenSource _taskCts;
		private IResearchService _researchService;
		public void Init(WorkerAnimator animator) {
			_animator = animator;
			_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
			_researchService = ServiceLocator.Container.Single<IResearchService>();
		}

		public void Research(ResearchStation researchStation) {
			if (_isResearching) {
				return;
			}

			_taskCts = new CancellationTokenSource();
			DoResearch(researchStation, _taskCts.Token).Forget();
		}

		private async UniTaskVoid DoResearch(ResearchStation researchStation, CancellationToken token) {
			_isResearching = true;
			_animator.PlayCraft();
			while (!token.IsCancellationRequested) {
				await _asyncRunner.Wait(researchTime, token);
				if (token.IsCancellationRequested) {
					break;
				}

				researchStation.AddResearchPoints(1);
			}
		}

		public void Cancel() {
			if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
				_taskCts.Cancel();
				_taskCts.Dispose();
				_taskCts = null;
				_animator.ResetToIdle();
				_isResearching = false;
			}
		}
	}
}