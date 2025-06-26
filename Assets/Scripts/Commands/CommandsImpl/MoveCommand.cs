using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand {
	public int Id { get; }
	public bool IsCompleted { get; private set; }
	
	private readonly ICoroutineRunner _coroutineRunner;
	private readonly IPathfindService _pathfinder;

	private List<Vector2> _path;
	public CommandPerformer Performer { get; private set; }
	public Vector2 TargetPosition { get; private set; }
	
	private Coroutine _coroutine;
	
	public MoveCommand(MoveCommandParams commandParams, ICoroutineRunner runner, IPathfindService pathfinder) {
		Performer = commandParams.Performer;
		TargetPosition = commandParams.TargetPosition;
		_coroutineRunner = runner;
		_pathfinder = pathfinder;
	}
	public void Execute() {
		if (_path == null)
			_path = _pathfinder.FindPath(new Vector2((int)Performer.transform.position.x, (int)Performer.transform.position.y), TargetPosition);

		if (new Vector2(Performer.transform.position.x, Performer.transform.position.y) == TargetPosition) {
			IsCompleted = true;
			return;
		}

		var indexOf = _path.IndexOf(new Vector2((int)Performer.transform.position.x, (int)Performer.transform.position.y));
		var nextStep = _path[indexOf + 1];
		if (_coroutine == null)
			_coroutine = _coroutineRunner.StartCoroutine(GoTo(nextStep));
	}
	private IEnumerator GoTo(Vector2 nextStep) {
		Vector3 target = new Vector3(nextStep.x, nextStep.y, 0);
		yield return _coroutineRunner.StartCoroutine(LerpFromTo(Performer.transform.position, target, 1f));
		_coroutine = null;
	}

	private IEnumerator LerpFromTo(Vector3 from, Vector3 to, float time) {
		float elapsedTime = 0f;
		while (elapsedTime < time) {
			float t = elapsedTime / time;
			t = Mathf.SmoothStep(0f, 1f, t);
			Performer.transform.localPosition = Vector3.Lerp(from, to, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		Performer.transform.position = to;
	}

	public bool IsAvailable() {
		throw new System.NotImplementedException();
	}
}