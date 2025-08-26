using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Searcher : MonoBehaviour, ISearcher {
    [Header("Searcher Settings")]
    public float searchCooldown = 0.5f;

    public float searchTime = 1.5f;
    private float _lastSearchTime;
    private bool _isSearching;

    private WorkerAnimator _animator;
    private IAsyncRunner _asyncRunner;
    private CancellationTokenSource _taskCts;

    public void Init(WorkerAnimator animator) {
        _animator = animator;
        _asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
    }

    public void Search(CommandTarget target) {
        if (_isSearching) {
            return;
        }

        _taskCts = new CancellationTokenSource();
        DoSearch(target, _taskCts.Token).Forget();
    }

    private async UniTaskVoid DoSearch(CommandTarget target, CancellationToken token) {
        _isSearching = true;
        _animator.PlaySearch();
        while (!token.IsCancellationRequested) {
            await _asyncRunner.Wait(searchTime, token);
            if (token.IsCancellationRequested) {
                break;
            }

            target.Search();
        }
    }

    public void Cancel() {
        if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
            _taskCts.Cancel();
            _taskCts.Dispose();
            _taskCts = null;
            _animator.ResetToIdle();
            _isSearching = false;
        }
    }
}