using AI.Behaviors;
using AI.Node;
using AI.Node.Jobs;

namespace AI {
    public class BTRoot_Settler : ParallelSelector {
        public BTRoot_Settler(Settler settler, ICommandService commands, ICraftingService crafting,
            IResourceManager resources, IBuildingService building, IFarmingService farming, ITimeScaleService timeScale) {
            var nonTactical = new Selector();
            //находится в обычном режиме
            nonTactical
                .AddChild(new Conditional(() => settler.Data.tactical.IsTactical))
                //.AddChild(new Behavior_Death(settler))
                .AddChild(new Behavior_CriticalTired(settler))
                .AddChild(new Behavior_Energy(settler))         //Идет в кровать
                .AddChild(new Behavior_Care(settler))   
                .AddChild(new Jobs(settler, commands))
                .AddChild(new Job_GiveCare(settler))
                .AddChild(new Job_ChargeTimeMachine(settler, timeScale))
                .AddChild(new Job_Craft(settler, crafting))
               
                //фермерство
                .AddChild(new Job_Harvest(settler,farming))
                .AddChild(new Job_Plant(settler,farming))
                .AddChild(new Job_WaterPlant(settler,farming))
                
                .AddChild(new Job_Build(settler, building))
                .AddChild(new Job_HaulResourceForCrafting(settler, crafting, resources))
                .AddChild(new Job_HaulResourceForBuilding(settler, building, resources))
                .AddChild(new Behavior_Idle(settler));

            //находится в тактическом режиме
            AddChild(new Behavior_Tactical(settler, resources));
            AddChild(nonTactical);
        }
    }
}