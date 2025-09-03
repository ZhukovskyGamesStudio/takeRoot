using System;

namespace AI.Node.Jobs {
    public class Job_Search : Sequence {
        public Job_Search(Settler settler) {
            SettlerData data = settler.Data;
            Func<bool> condition = () => Jobs.JobCondition(data, JobType.Search);

            ConditionalAction move = new ConditionalAction().Do(new Action_MoveTo(settler)).While(condition);
            ConditionalAction search = new ConditionalAction().Do(new Action_SearchObject(settler)).While(condition);

            AddChild(move);
            AddChild(search);
        }
    }
}