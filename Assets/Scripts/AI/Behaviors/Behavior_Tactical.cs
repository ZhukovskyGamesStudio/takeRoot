using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Tactical : Sequence {
        public Behavior_Tactical(Settler settler) {
            
            AddChild(new Conditional(() => settler.Data.IsTactical));
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(() => settler.Data.IsTactical);
            AddChild(move);
        }
    }
}