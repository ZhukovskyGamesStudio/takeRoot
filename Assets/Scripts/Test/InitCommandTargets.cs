using System;
using UnityEngine;

namespace Settlers.Test {
	public class InitCommandTargets : MonoBehaviour {
		private void Start() {
			var id = 0;
			foreach (var target in FindObjectsByType<CommandTarget>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
				target.Data.Id = ++id;
			}
		}
	}
}