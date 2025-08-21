using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI {
	[Serializable]
	public class SettlerData {
		public bool IsTactical;
		[Header("Jobs")]
		public JobType currJob;
		public CommandTarget currTarget;
		public CommandTarget subsequentTarget;
		public bool HasMovePos;
		public Vector3 curMovePos;
		
		public bool HasJob => currJob != JobType.None;
		public bool CanDoJob => true; //TODO: make condition
		
		
		public CommandTarget ItemInHands;
		public bool HasItem => ItemInHands != null;


		[Header("Energy")]
		public Settler_EnergyData energy;
		

		[Header("Idle move")]
		public float IdleMoveCooldown;
		public float IdleMoveTimer;
		
		[Header("Hit")]
		public float HitTime = 1.3f;
	}
}