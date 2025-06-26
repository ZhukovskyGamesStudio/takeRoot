using System.Collections;
using UnityEngine;

public class DebugCommand: ICommand {
	private float _waitTime;
	private string _text;
	private Vector2 _pos;
	private readonly ICoroutineRunner _coroutineRunner;

	private Coroutine _waitCoroutine;
	private bool _waited;
	public int Id { get;}
	public bool IsCompleted { get; private set; }

	public DebugCommand(DebugCommandParams commandParams, ICoroutineRunner coroutineRunner) {
		_waitTime = commandParams.Wait;
		_text = commandParams.Text;
		_pos = commandParams.At;
		_coroutineRunner = coroutineRunner;
	}

	public void Execute() {
		_waitCoroutine ??= _coroutineRunner.StartCoroutine(WaitSeconds(_waitTime));

		if (_waited) {
			Debug.Log($"{_text} called at {_pos}");
			IsCompleted = true;
		}
	}

	private IEnumerator WaitSeconds(float waitTime) {
		var time = waitTime;
		while (time > 0) {
			time -= Time.deltaTime;
			yield return null;
		}
		_waited = true;
	}

	public bool IsAvailable() {
		throw new System.NotImplementedException();
	}
}