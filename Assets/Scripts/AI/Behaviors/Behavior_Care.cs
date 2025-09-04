
using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Care : Sequence {
        public Behavior_Care(Settler settler) {
            AddChild(
                new Conditional(() => settler.Data.needs.Value.CareData.LowCare));

            Selector hasCareStation = new Selector()
                .AddChild(new Conditional(() => settler.Data.targets.SitOnCareStation != null))
                .AddChild(new Action_TryClaimCareStation(settler));

            Sequence waitOnCareStationWhenLowcare = new Sequence()
                .AddChild(new Conditional(() => settler.Data.needs.Value.CareData.LowCare))
                .AddChild(hasCareStation)
                .AddChild(new Action_GoToCareStation(settler))
                .AddChild(new ConditionalAction()
                    .Do(new Action_MoveToPos(settler))
                    .While(() => settler.Data.needs.Value.CareData.LowCare && settler.Data.targets.SitOnCareStation != null))
                .AddChild(new Action_ReceiveCare(settler));

            Selector careBehaviour = new Selector()
                .AddChild(waitOnCareStationWhenLowcare);

            AddChild(careBehaviour);
        }
    }
}