using System.Threading;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DinamoCharger : MonoBehaviour, IDinamoCharger {
    public float chargeTime = 2f;
    private bool _isCrafting;
    private WorkerAnimator _animator;
    private IAsyncRunner _asyncRunner;
    private CancellationTokenSource _taskCts;
    private DinamoMachine _dinamoMachine;

    public void Init(WorkerAnimator animator) {
        _animator = animator;
        _asyncRunner = ServiceLocator.Container.Single<IAsyncRunner>();
    }

    public void Charge(DinamoMachine craftingStation) {
        if (_isCrafting) {
            return;
        }

        _taskCts = new CancellationTokenSource();
        DoCharge(craftingStation, _taskCts.Token).Forget();
    }

    private async UniTaskVoid DoCharge(DinamoMachine dinamoMachine, CancellationToken token) {
        _isCrafting = true;
        _animator.PlayCraft();
        _dinamoMachine = dinamoMachine;
        dinamoMachine.PlayWorkAnimation();
        while (!token.IsCancellationRequested) {
            await _asyncRunner.Wait(chargeTime, token);
            if (token.IsCancellationRequested) {
                break;
            }

            dinamoMachine.Charge();
        }
    }

    public void Cancel() {
        if (_taskCts != null && !_taskCts.Token.IsCancellationRequested) {
            _taskCts.Cancel();
            _taskCts.Dispose();
            _taskCts = null;
            if (_dinamoMachine != null) {
                _dinamoMachine.PlayIdleAnimation();
            }
            _animator.ResetToIdle();
            _isCrafting = false;
        }
    }
}