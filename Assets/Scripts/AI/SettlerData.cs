using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI {
	[Serializable]
	public class SettlerData {
		public bool IsTactical;
		
		public JobType currJob;
		public CommandTarget currTarget;
		public CommandTarget subsequentTarget;

		public bool HasMovePos;
		public Vector3 curMovePos;

		public float IdleMoveCooldown = 11;
		public float IdleMoveTimer;
		
		public float HitTime = 1.3f;

		public CommandTarget ItemInHands;
		
		public bool HasJob => currJob != JobType.None;
		public bool CanDoJob => true; //TODO: make condition
		public bool HasItem => ItemInHands != null;


		[Header("Energy")]
		public int maxEnergy;
		public int currentEnergy;
		public int tiredThreshold;
		public int criticalTiredThreshold;
		public int energyChange;
		public int defaultEnergyChange;
		public int onGroundEnergyChange;
		public float energyCooldown;
		public float energyTimer;
		public bool isSleeping;
		public Bed bed;
		public bool HasOwnBed => bed != null; //TODO: make own bed data
		public bool IsTired => currentEnergy < tiredThreshold;
		public bool IsCriticalTired => currentEnergy < criticalTiredThreshold;
		


	}
}