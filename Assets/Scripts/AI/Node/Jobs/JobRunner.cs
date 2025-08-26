using System;
using AI.Node.Conditions;

namespace AI.Node.Jobs {
    public abstract class JobRunner : Sequence {
        protected Settler Settler;
        protected JobType JobType = JobType.None;

        protected JobRunner(Settler settler) {
            Settler = settler;
        }

        protected void InsertOnFailed(Func<bool> condition) {
            Conditional conditional = new(condition);
            InsertChild(0, new FailConditionAction(conditional, OnFailedAction));
        }

        protected abstract void OnFailedAction();
    }
}