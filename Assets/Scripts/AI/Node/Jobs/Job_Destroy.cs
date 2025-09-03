using System;

namespace AI.Node.Jobs {
    public class Job_Destroy : Sequence {
        private Settler _settler;

        public Job_Destroy(Settler settler) {
            SettlerData data = settler.Data;
            Func<bool> condition = () => Jobs.JobCondition(data, JobType.Destroy);
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveTo(settler)).While(condition);
            ConditionalAction hit = new ConditionalAction().Do(new Action_Hit(settler)).While(condition);

            AddChild(move);
            AddChild(hit);
        }
    }
}