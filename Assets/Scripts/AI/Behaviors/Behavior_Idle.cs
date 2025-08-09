using System.Collections.Generic;
using AI.Node;
using AI.Node.Jobs;
using UnityEngine;

namespace AI.Behaviors {
	public class Behavior_Idle : Sequence {
		public Behavior_Idle(Settler settler) : base(new List<BTNode>() {
			new DoUntil(() => settler.Data.IdleMoveTimer += Time.deltaTime, 
				() => settler.Data.IdleMoveTimer >= settler.Data.IdleMoveCooldown),
			new Selector(new List<BTNode>() {
				new Conditional(() => settler.Data.HasMovePos),
				new Action_PickRandomPos(settler)
			}),
			new Action_MoveToPos(settler),
			new Action_ClearMovePos(settler)
		}) { }
	}
}