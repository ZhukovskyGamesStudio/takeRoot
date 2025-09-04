using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Electricity : Sequence {
        public Behavior_Electricity(Settler settler) {
            AddChild(new Conditional(() => settler.Data.names.Race == Race.Robots));
            AddChild(new Conditional(() => settler.Data.needs.Value.SatietyData.LowSatiety));

            Selector hascooler = new Selector()
                //.AddChild(new Conditional(() => settler.Data.needs.Value.HasOwnBed))
                .AddChild(new Action_TryFindElectricitySource(settler));

            Sequence consumeElectricity = new Sequence()
                //.AddChild(new Conditional(() => settler.Data.needs.Value.Energy.IsTired))
                .AddChild(hascooler)
                //.AddChild(new Action_GetFreePosNearBed(settler))
                .AddChild(new ConditionalAction().Do(new Action_MoveToPos(settler)).While(() =>
                    settler.Data.needs.Value.SatietyData.LowSatiety && settler.Data.targets.ElectricitySource != null))
                .AddChild(new Action_ConsumeElectricity(settler));

            Selector consumeBehaviour = new Selector().AddChild(consumeElectricity);

            AddChild(consumeBehaviour);
        }
    }
}