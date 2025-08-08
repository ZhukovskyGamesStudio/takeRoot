using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_FindJob : BTNode {
		private Settler _settler;

		public Action_FindJob(Settler settler) {
			_settler = settler;
		}

		public override BTNodeState Evaluate() {
			var obj = Object.FindObjectsByType<CommandTarget>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(s => !s.Searched);
			if (obj == null) return BTNodeState.Failure;
			_settler.Data.currJob = JobType.Search;
			_settler.Data.currTarget = obj;
			return BTNodeState.Success;
		}
	}
}