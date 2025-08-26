using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class UniTaskAsyncRunner : IAsyncRunner {
    public async UniTask Wait(float seconds, CancellationToken cts = default) {
        await UniTask.Delay(TimeSpan.FromSeconds(seconds));
    }

    public async UniTask WaitAndDo(float seconds, Action action) {
        await UniTask.Delay(TimeSpan.FromSeconds(seconds));
        action?.Invoke();
    }

    public async UniTask WaitUntilAndDo(Func<bool> predicate, Action action) {
        await UniTask.WaitUntil(predicate);
        action?.Invoke();
    }

    public async UniTask WaitUntil(Func<bool> predicate, CancellationToken cts = default) {
        await UniTask.WaitUntil(predicate, cancellationToken: cts);
    }
}