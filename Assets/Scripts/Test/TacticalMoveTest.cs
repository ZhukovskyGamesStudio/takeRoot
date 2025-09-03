using System;
using System.Collections.Generic;
using CodeBase.Services;
using UnityEngine;

namespace Settlers.Test {
	public class TacticalMoveTest : MonoBehaviour {
		public AI.Settler settlerToTest;
		public Transform testTacticalMovePos;
		private ITacticalService _tacticalService;
		

		private void CheckService() {
			if (_tacticalService == null) {
				_tacticalService = ServiceLocator.Container.Single<ITacticalService>();
			}
		}

		public void SetSelectedSettler() {
			CheckService();
		}

		public void SetSelectedTactical() {
			CheckService();
			_tacticalService.SetTacticalForSelectedSettlers();
		}

		public void AddTacticalMovePos() {
			CheckService();
			var pos = testTacticalMovePos.position;
			_tacticalService.AddTacticalMovePosToSelectedSettlers(pos);
		}
	}
}