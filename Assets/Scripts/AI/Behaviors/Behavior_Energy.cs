using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Energy : Sequence {
        public Behavior_Energy(Settler settler) {
            AddChild(
                new Conditional(() => settler.Data.needs.Energy.IsCriticalTired || settler.Data.needs.Energy.IsTired || settler.Data.needs.Energy.isSleeping));

            Selector hasBed = new Selector().AddChild(new Conditional(() => settler.Data.needs.Energy.HasOwnBed))
                .AddChild(new Action_TryClaimBed(settler));

            Sequence sleepOnBedWhenTired = new Sequence().AddChild(new Conditional(() => settler.Data.needs.Energy.IsTired)).AddChild(hasBed)
                .AddChild(new Action_GetFreePosNearBed(settler))
                .AddChild(new ConditionalAction().Do(new Action_MoveToPos(settler))
                    .While(() => settler.Data.needs.Energy.IsTired && settler.Data.needs.Energy.HasOwnBed)).AddChild(new Action_Sleep(settler));

            Selector sleepBehavior = new Selector().AddChild(sleepOnBedWhenTired);

            AddChild(sleepBehavior);
        }
    }
}