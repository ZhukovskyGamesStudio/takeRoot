using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
	public class Behavior_Tactical : Sequence {
		public Behavior_Tactical(Settler settler, IResourceManager resourceManager) {
			var tData = settler.Data.tactical;
			var tacticalMove = new Sequence()
				.AddChild(new Conditional(() => tData.HasTacticalMovePos));
				//.AddChild(TacticalMove)
			
			var tactical = new Sequence()
				.AddChild(new Conditional(() => settler.Data.tactical.IsTactical))
				.AddChild(new Inverter(new Action_ClearHaulBuilding(settler, resourceManager)))
				.AddChild(new Inverter(new ResetJobOnSettler(settler)));
			
			var mode = new Selector()
				.AddChild(tactical)
				.AddChild(new Action_ClearTacticalData(settler));
		}
	}
}