using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Waterer : MonoBehaviour, IWaterer {
	[Header("Waterer Settings")]
	public float waterAmount = 15f;
	public float waterTime = 1.2f;
	private float _lastWaterTime;
	private bool _isWatering;
	
	private WorkerAnimator _animator;
	private IAsyncRunner _asyncRunner;
	private CancellationTokenSource _taskCts;

	public void Init(WorkerAnimator animator) {
		_animator = animator;
		_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
	}

	public void Water(CommandTarget target) {
		if (_isWatering) return;

		_taskCts = new CancellationTokenSource();
		RotateToTarget(target);
		//_animator.ResetToIdle();
		DoWater(target, _taskCts.Token).Forget();
	}

	private async UniTaskVoid DoWater(CommandTarget target, CancellationToken token) {
		_isWatering = true;
		_animator.PlayWater();
		while (!token.IsCancellationRequested) {
			await _asyncRunner.Wait(waterTime);
			target.Water(waterAmount);
		}
	}
	

	public void Cancel() {
		if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
			_taskCts.Cancel();
			_taskCts.Dispose();
			_taskCts = null;
			_animator.ResetToIdle();
			_isWatering = false;
		}
	}

	private void RotateToTarget(CommandTarget target) {
		Vector3 diff = target.transform.localPosition - transform.localPosition;
		if (diff.x < 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
		}

		if (diff.x > 0) {
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
	}
}