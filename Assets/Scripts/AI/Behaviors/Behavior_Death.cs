using AI.Node;
using AI.Node.Jobs;

namespace AI.Behaviors {
    public class Behavior_Death : Sequence {
        public Behavior_Death(Settler settler) {
            AddChild(new Conditional(() => !Settler.Immortal));
            Sequence hungerDeath = new Sequence()
                .AddChild(new Conditional(() => settler.Data.needs.SatietyData.currentSatiety == 0))
                .AddChild(new Action_Die(settler, DeathCause.Hunger));
            
            Sequence careDeath = new Sequence()
                .AddChild(new Conditional(() => settler.Data.needs.CareData.currentCare == 0))
                .AddChild(new Action_Die(settler, DeathCause.Care));
            
            Selector behaviour = new Selector()
                .AddChild(hungerDeath)
                .AddChild(careDeath);
            
            AddChild(behaviour);
        }
    }
}