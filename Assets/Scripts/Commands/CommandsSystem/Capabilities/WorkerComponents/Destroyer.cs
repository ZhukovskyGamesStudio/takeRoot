using System.Threading.Tasks;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Destroyer : MonoBehaviour, IDestroyer {
	[Header("Destroyer Settings")]
	public float damage = 25f;
	public float hitCooldown = 0.5f;
	
	private float _lastHitTime;
	private WorkerAnimator _animator;
	private IAsyncRunner _asyncRunner;
	private bool _isDestroying;
	private bool _hit;

	public void Init(WorkerAnimator animator) {
		_animator = animator;
		_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
		_animator.StateExited += state => {
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

	public void StartHit(CommandTarget target) {
		if (_isDestroying) return;
		if (OnCooldown()) return;

		DoHit(target).Forget();
	}

	private bool OnCooldown() {
		return Time.time - _lastHitTime < hitCooldown;
	}

	private async UniTaskVoid DoHit(CommandTarget target) {
		_isDestroying = true;
		_animator.PlayHit();
		await _asyncRunner.WaitUntil(() => _hit);
		target.TakeDamage(damage);
		_lastHitTime = Time.time;
		_isDestroying = false;
	}
	
	
} 