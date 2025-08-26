using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
public class Crafter : MonoBehaviour, ICrafter {
	public float craftTime = 2f;
	private bool _isCrafting;
	private WorkerAnimator _animator;
	private IAsyncRunner _asyncRunner;
	private CancellationTokenSource _taskCts;

	public void Init(WorkerAnimator animator) {
		_animator = animator;
		_asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
	}

	public void Craft(CraftingStation craftingStation) {
		if (_isCrafting) {
			return;
		}

		_taskCts = new CancellationTokenSource();
		DoCraft(craftingStation, _taskCts.Token).Forget();
	}

	private async UniTaskVoid DoCraft(CraftingStation craftingStation, CancellationToken token) {
		_isCrafting = true;
		_animator.PlayCraft();
		while (!token.IsCancellationRequested) {
			await _asyncRunner.Wait(craftTime, token);
			if (token.IsCancellationRequested) {
				break;
			}

			craftingStation.Craft();
		}
	}

	public void Cancel() {
		if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
			_taskCts.Cancel();
			_taskCts.Dispose();
			_taskCts = null;
			_animator.ResetToIdle();
			_isCrafting = false;
		}
	}
}