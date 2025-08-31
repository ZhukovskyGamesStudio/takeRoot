using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
public class TimeMachineCharger : MonoBehaviour, ITimeMachineCharger {
	[SerializeField]
	private float _chargeTime;
	
	private bool _isCharging;
	private WorkerAnimator _animator;
	private IAsyncRunner _asyncRunner;
	private CancellationTokenSource _taskCts;

	public void Init(WorkerAnimator animator) {
		_animator = animator;
		_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
	}

	public void Charge(TimeMachine timeMachine, Race race) {
		if (_isCharging) {
			return;
		}

		_taskCts = new CancellationTokenSource();
		DoCharge(timeMachine, race, _taskCts.Token).Forget();
	}

	private async UniTaskVoid DoCharge(TimeMachine timeMachine, Race race, CancellationToken token) {
		_isCharging = true;
		_animator.PlayCraft();
		
		while (!token.IsCancellationRequested) {
			await _asyncRunner.Wait(_chargeTime, token);
			if (token.IsCancellationRequested) {
				break;
			}

			timeMachine.Charge(race);
		}
	}

	public void Cancel() {
		if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
			_taskCts.Cancel();
			_taskCts.Dispose();
			_taskCts = null;
			_animator.ResetToIdle();
			_isCharging = false;
		}
	}
}