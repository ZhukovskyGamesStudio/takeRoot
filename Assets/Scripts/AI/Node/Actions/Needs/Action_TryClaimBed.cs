using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_TryClaimBed : BTNode {
        private readonly Settler _settler;

        public Action_TryClaimBed(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            Bed freeBed = Object.FindObjectsByType<Bed>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(b => b.IsFree);
            if (freeBed == null) {
                return _state = BTNodeState.Failure;
            }

            freeBed.SetSettler(_settler);
            return _state = BTNodeState.Success;
        }
    }
}