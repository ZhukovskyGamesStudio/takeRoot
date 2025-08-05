using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Destroyer : MonoBehaviour, IDestroyer {
	[Header("Destroyer Settings")]
	public float damage = 25f;
	public float hitCooldown = 0.5f;
	public float hitSpeedMultiplier = 1f;
	
	private float _lastHitTime;
	private WorkerAnimator _animator;
	private IAsyncRunner _asyncRunner;
	private bool _isDestroying;
	private bool _hit;
	private CancellationTokenSource _taskCts;

	public void Init(WorkerAnimator animator) {
		_animator = animator;
		_animator.SetHitSpeedMultiplier(hitSpeedMultiplier);
		_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
		_animator.StateExited += state => { // TODO: unsubscribe on destroyed
			if (state == AnimatorState.Hit) {
				_hit = true;
			}
		};
		_animator.StateEntered += state => {
			if (state == AnimatorState.Hit) {
				_hit = false;
			}
		};
	}

	public void Hit(CommandTarget target) {
		if (_isDestroying) return;
		if (OnCooldown()) return;
	
		_taskCts = new CancellationTokenSource();
		DoHit(target, _taskCts.Token).Forget();
	}

	private bool OnCooldown() {
		return Time.time - _lastHitTime < hitCooldown;
	}

	private async UniTaskVoid DoHit(CommandTarget target, CancellationToken token) {
		_isDestroying = true;
		_animator.PlayHit();
		await _asyncRunner.WaitUntil(() => _hit, token);
		target.TakeDamage(damage);
		_lastHitTime = Time.time;
		_isDestroying = false;
	}
	public void Cancel() {
		if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
			_taskCts.Cancel();
			_taskCts.Dispose();
			_taskCts = null;
			_animator.ResetToIdle();
			_isDestroying = false;
		}
	}
} 