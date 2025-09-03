using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI {
	[Serializable]
	public class ZombieData {
		public Vector3 CurrMovePos;

		public Rect DetectArea;
		public int DetectRange;
		public Settler Target;
		
		public float PatrolCooldown;
		public float PatrolTimer;
	}
}