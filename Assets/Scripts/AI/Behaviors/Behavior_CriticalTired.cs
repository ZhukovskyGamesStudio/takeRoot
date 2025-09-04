using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_CriticalTired : Sequence {
        public Behavior_CriticalTired(Settler settler) {
            AddChild(new Conditional(() => settler.Data.needs.Value.Energy.IsCriticalTired));
            AddChild(new Action_SleepOnGround(settler));
        }
    }
}