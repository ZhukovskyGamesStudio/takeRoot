using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public interface IAsyncRunner : IService {
	UniTask Wait(float seconds, CancellationToken cancellationToken = default);
	UniTask WaitAndDo(float seconds, Action action);
	UniTask WaitUntilAndDo(Func<bool> predicate, Action action);
	UniTask WaitUntil(Func<bool> predicate, CancellationToken cancellationToken = default);
}