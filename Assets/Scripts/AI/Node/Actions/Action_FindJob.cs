using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_FindJob : BTNode {
		private Settler _settler;
		private readonly ICommandService _commands;

		public Action_FindJob(Settler settler, ICommandService commands) {
			_settler = settler;
			_commands = commands;
		}

		public override BTNodeState Evaluate() {
			var obj = _commands.GetJob();
			if (obj == null) return _state = BTNodeState.Failure;
			obj.Reserved = true;
			_settler.Data.currJob = obj.CurrentJobType;
			_settler.Data.currTarget = obj;
			return _state = BTNodeState.Success;
		}
	}
}