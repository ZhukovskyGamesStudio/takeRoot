using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Death : Sequence {
        public Behavior_Death(Settler settler) {
            AddChild(
                new Conditional(() => settler.Data.needs.SatietyData.LowSatiety));

            Sequence hungerDeath = new Sequence()
                .AddChild(new Conditional(() => settler.Data.needs.SatietyData.currentSatiety == 0))
                .AddChild(new Action_Die(settler));

            Selector behaviour = new Selector()
                .AddChild(hungerDeath);
            
            AddChild(behaviour);
        }
    }
}