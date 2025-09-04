using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
	public class Behavior_Tactical : Sequence {
		public Behavior_Tactical(Settler settler, IResourceManager resourceManager) {
			var data = settler.Data.tactical;
			var attack = new Sequence()
				.AddChild(new Conditional(() => data.Target))
				.AddChild(new Action_PickPosNearZombie(settler))
				.AddChild(new ConditionalAction()
					.Do(new Action_TacticalMove(settler))
					.While(() => data.HasTacticalMovePos && data.IsTactical && data.Target && data.TacticalMovePos == data.Target.Position))
					.AddChild(new Action_TacticalAttack(settler));

			var tactical = new Sequence()
				.AddChild(new Conditional(() => data.IsTactical))
				.AddChild(new Inverter(new ResetJobOnSettler(settler)))
				.AddChild(new Inverter(new Action_ClearHaulBuilding(settler, resourceManager)))
				.AddChild(new Inverter(new Action_ClearBuilding(settler)))
				.AddChild(new Inverter(new Action_ClearHaulCrafting(settler, resourceManager)));
				
			var mode = new Selector()
				.AddChild(attack)
				.AddChild(new ConditionalAction()
					.Do(new Action_TacticalMove(settler))
					.While(() => data.HasTacticalMovePos && data.IsTactical && !data.Target))
				.AddChild(new Action_ClearTacticalData(settler));

			AddChild(new Sequence()
				.AddChild(tactical)
				.AddChild(mode));
		}
	}
}