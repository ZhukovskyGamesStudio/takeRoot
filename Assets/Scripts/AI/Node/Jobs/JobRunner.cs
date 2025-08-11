using System;
using System.Collections.Generic;
using AI.Node.Conditions;

namespace AI.Node.Jobs {
	public abstract class JobRunner : Sequence {
		protected Settler Settler;
		protected JobType JobType = JobType.None;
		protected JobRunner(Settler settler){
			Settler = settler;
			InsertChild(0, new Conditional(() => Settler.Data.currJob == JobType));
		}

		protected void SetOnFailed(Func<bool> condition) {
			var conditional = new Conditional(condition);
			InsertChild(1, new FailConditionAction(conditional, OnFailedAction));
		}
		protected abstract void OnFailedAction();
	}
}