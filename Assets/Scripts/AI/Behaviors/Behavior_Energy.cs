using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
	public class Behavior_Energy : Sequence {
		
		public Behavior_Energy(Settler settler) {
			AddChild(new Conditional(() => settler.Data.energy.IsCriticalTired 
			                               || settler.Data.energy.IsTired 
			                               || settler.Data.energy.isSleeping));

			var hasBed = new Selector()
				.AddChild(new Conditional(() => settler.Data.energy.HasOwnBed))
				.AddChild(new Action_TryClaimBed(settler));

			var sleepOnBedWhenTired = new Sequence()
				.AddChild(new Conditional(() => settler.Data.energy.IsTired))
				.AddChild(hasBed)
				.AddChild(new Action_GetFreePosNearBed(settler))
				.AddChild(new ConditionalAction()
					.Do(new Action_MoveToPos(settler))
					.While(() => settler.Data.energy.IsTired &&
					             settler.Data.energy.HasOwnBed))
				.AddChild(new Action_Sleep(settler));
			

			var sleepBehavior = new Selector()
				.AddChild(sleepOnBedWhenTired);

			AddChild(sleepBehavior);
		}
	}
}