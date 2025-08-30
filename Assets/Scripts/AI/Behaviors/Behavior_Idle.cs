using AI.Node;
using AI.Node.Jobs;
using UnityEngine;

namespace AI.Behaviors {
    public class Behavior_Idle : Sequence {
        public Behavior_Idle(Settler settler) {
            Selector pickMovePos = new Selector().AddChild(new Conditional(() => settler.Data.HasMovePos))
                .AddChild(new Action_PickRandomPos(settler));
            AddChild(pickMovePos);
            AddChild(new Action_MoveToPos(settler));

            var move = new ConditionalAction()
                .Do(new Action_MoveToPos(settler))
                .While(() => settler.Data.IsIdle);
            AddChild(new Action_ClearMovePosAfterIdleMove(settler));
        }
    }
}