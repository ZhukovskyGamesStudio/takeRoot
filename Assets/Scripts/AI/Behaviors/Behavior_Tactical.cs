using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
	public class Behavior_Tactical : Sequence {
		public Behavior_Tactical(Settler settler, IResourceManager resourceManager) {
			var tData = settler.Data.tactical;
			var attack = new Sequence()
				.AddChild(new Conditional(() => !settler.Data.tactical.Target))
				.AddChild(new Action_PickPosNearZombie(settler))
				.AddChild(new ConditionalAction()
					.Do(new Action_TacticalMove(settler))
					.While(() => settler.Data.tactical.HasTacticalMovePos && settler.Data.tactical.IsTactical && settler.Data.tactical.Target));
			var tactical = new Sequence()
				.AddChild(new Conditional(() => settler.Data.tactical.IsTactical))
				.AddChild(new Inverter(new ResetJobOnSettler(settler)))
				.AddChild(new Inverter(new Action_ClearHaulBuilding(settler, resourceManager)))
				.AddChild(new Inverter(new Action_ClearBuilding(settler)))
				.AddChild(new Inverter(new Action_ClearHaulCrafting(settler, resourceManager)))
				.AddChild(new ConditionalAction()
					.Do(new Action_TacticalMove(settler))
					.While(() => settler.Data.tactical.HasTacticalMovePos && settler.Data.tactical.IsTactical));
				
			var mode = new Selector()
				.AddChild(tactical)
				.AddChild(new Action_ClearTacticalData(settler));

			AddChild(mode);
		}
	}
}