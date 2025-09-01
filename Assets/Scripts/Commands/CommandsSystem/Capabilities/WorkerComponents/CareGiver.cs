using UnityEngine;
using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CareGiver : MonoBehaviour, ICareGiver {
    [Header("CareGiver Settings")]
    public float CareTime = 0.5f;

    private bool _isPerforming;

    private WorkerAnimator _animator;
    private IAsyncRunner _asyncRunner;
    private CancellationTokenSource _taskCts;

    public void Init(WorkerAnimator animator) {
        _animator = animator;
        _asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
    }

    public void Care(CareStation target) {
        if (_isPerforming) {
            return;
        }

        _taskCts = new CancellationTokenSource();
        DoCare(target, _taskCts.Token).Forget();
    }

    private async UniTaskVoid DoCare(CareStation target, CancellationToken token) {
        _isPerforming = true;
        _animator.PlayCraft();
        while (!token.IsCancellationRequested) {
            await _asyncRunner.Wait(CareTime, token);
            if (token.IsCancellationRequested) {
                break;
            }

            target.AddCare();
        }
    }

    public void Cancel() {
        if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
            _taskCts.Cancel();
            _taskCts.Dispose();
            _taskCts = null;
            _animator.ResetToIdle();
            _isPerforming = false;
        }
    }
}