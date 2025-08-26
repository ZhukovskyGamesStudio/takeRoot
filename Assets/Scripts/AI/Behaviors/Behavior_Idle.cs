using AI.Node;
using AI.Node.Jobs;
using UnityEngine;

namespace AI.Behaviors {
    public class Behavior_Idle : Sequence {
        public Behavior_Idle(Settler settler) {
            AddChild(new DoUntil(() => settler.Data.IdleMoveTimer += Time.deltaTime,
                () => settler.Data.IdleMoveTimer >= settler.Data.IdleMoveCooldown));

            Selector pickMovePos = new Selector().AddChild(new Conditional(() => settler.Data.HasMovePos))
                .AddChild(new Action_PickRandomPos(settler));
            AddChild(pickMovePos);
            AddChild(new Action_MoveToPos(settler));
            AddChild(new Action_ClearMovePos(settler));
        }
    }
}