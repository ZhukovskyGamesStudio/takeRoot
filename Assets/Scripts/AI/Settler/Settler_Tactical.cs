using System;
using UnityEngine;

namespace AI {
	[Serializable]
	public class Settler_Tactical {
		public bool IsTactical;
		public Vector3 TacticalMovePos;
		public bool HasTacticalMovePos;
		public Zombie Target;
	}
}