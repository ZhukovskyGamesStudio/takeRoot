using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Tactical : Sequence {
        public Behavior_Tactical(Settler settler, IResourceManager resourceManager) {
            AddChild(new Conditional(() => settler.Data.tactical.IsTactical));
            AddChild(new Inverter(new Action_ClearHaulBuilding(settler, resourceManager)));
            AddChild(new Inverter(new ResetJobOnSettler(settler)));
        }
    }
}