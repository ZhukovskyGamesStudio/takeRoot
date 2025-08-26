using System;

namespace AI.Node.Jobs {
    public class Job_Search : Sequence {
        public Job_Search(Settler settler) {
            SettlerData data = settler.Data;
            Func<bool> condition = () => data.currTarget &&
                                         data.currTarget.Data.CurrentJob == JobType.Search && data.currJob == JobType.Search &&
                                         !data.IsTactical;

            ConditionalAction move = new ConditionalAction().Do(new Action_MoveTo(settler)).While(condition);
            ConditionalAction search = new ConditionalAction().Do(new Action_SearchObject(settler)).While(condition);

            AddChild(move);
            AddChild(search);
        }
    }
}