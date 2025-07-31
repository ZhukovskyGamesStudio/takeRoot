using System;
using Cysharp.Threading.Tasks;

public interface IAsyncRunner : IService {
	UniTask WaitAndDo(float seconds, Action action);
	UniTask WaitUntilAndDo(Func<bool> predicate, Action action);
	UniTask WaitUntil(Func<bool> predicate);
}