using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI {
	[Serializable]
	public class ZombieData {
		public Settler Target;
		public Vector3 CurrMovePos;

		public float PatrolCooldown;
		public float PatrolTimer;
	}
}