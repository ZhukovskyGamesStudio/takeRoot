using AI.Behaviors;
using AI.Node;
using AI.Node.Jobs;

namespace AI {
    public class BTRoot_Settler : Selector {
        public BTRoot_Settler(Settler settler, ICommandService commands, ICraftingService crafting,
            IResourceManager resources, IBuildingService building) {
            AddChild(new Behavior_CriticalTired(settler));
            AddChild(new Behavior_Tactical(settler));
            AddChild(new Behavior_Energy(settler));
            AddChild(new Jobs(settler, commands));
            AddChild(new Job_Craft(settler, crafting));
            AddChild(new Job_Build(settler, building));
            AddChild(new Job_HaulResourceForCrafting(settler, crafting, resources));
            AddChild(new Job_HaulResourceForBuilding(settler, building, resources));
            //AddChild(new Behavior_Idle(settler));
        }
    }
}