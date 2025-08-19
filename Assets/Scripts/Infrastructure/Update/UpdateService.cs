using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateService : MonoBehaviour, IUpdateService, ICoroutineRunner
{
	private readonly List<IUpdatable> _updatables = new();

	public void Register(IUpdatable updatable) {
		if (!_updatables.Contains(updatable))
			_updatables.Add(updatable);
	}
	
	public void Unregister(IUpdatable updatable) {
		_updatables.Remove(updatable);
	}
	
	public void Update() {
		foreach (IUpdatable updatable in _updatables.ToArray()) {
			updatable.Update();
		}
	}
}