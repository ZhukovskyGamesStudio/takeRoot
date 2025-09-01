
using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Water : Sequence {
        public Behavior_Water(Settler settler) {
            AddChild(new Conditional(() => settler.Data.names.Race == Race.Plants));
            AddChild(new Conditional(() => settler.Data.needs.SatietyData.LowSatiety));

            Selector hascooler = new Selector()
                //.AddChild(new Conditional(() => settler.Data.needs.HasOwnBed))
                .AddChild(new Action_TryFindCoolerWithWater(settler));

            Sequence drinkWater = new Sequence()
                //.AddChild(new Conditional(() => settler.Data.needs.Energy.IsTired))
                .AddChild(hascooler)
                //.AddChild(new Action_GetFreePosNearBed(settler))
                .AddChild(new ConditionalAction()
                    .Do(new Action_MoveToPos(settler))
                    .While(() => settler.Data.needs.SatietyData.LowSatiety && settler.Data.needs.SatietyData.Cooler != null))
                .AddChild(new Action_DrinkWater(settler));

            Selector drinkBehaviour = new Selector()
                .AddChild(drinkWater);

            AddChild(drinkBehaviour);
        }
    }
}