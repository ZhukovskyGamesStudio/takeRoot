using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Farmer : MonoBehaviour, IPlanter {
    [Header("Farmer Settings")]
    public float PlantTime = 1.5f;
    private float _lastPlantTime;
    private bool _isPlanting;

    private WorkerAnimator _animator;
    private IAsyncRunner _asyncRunner;
    private CancellationTokenSource _taskCts;

    public void Init(WorkerAnimator animator) {
        _animator = animator;
        _asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
    }
    
    public void Plant(FarmingPlot target) {
        if (_isPlanting) {
            return;
        }

        _taskCts = new CancellationTokenSource();
        DoPlant(target, _taskCts.Token).Forget();
    }

    private async UniTaskVoid DoPlant(FarmingPlot target, CancellationToken token) {
        _isPlanting = true;
        _animator.PlaySleep();
        while (!token.IsCancellationRequested) {
            await _asyncRunner.Wait(PlantTime, token);
            if (token.IsCancellationRequested) {
                break;
            }

            target.PlantFinished();
        }
    }

    public void Cancel() {
        if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
            _taskCts.Cancel();
            _taskCts.Dispose();
            _taskCts = null;
            _animator.ResetToIdle();
            _isPlanting = false;
        }
    }

    
}