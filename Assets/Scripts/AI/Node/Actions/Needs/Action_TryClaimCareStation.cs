
using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_TryClaimCareStation : BTNode {
        private readonly Settler _settler;

        public Action_TryClaimCareStation(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            CareStation freeBed = Object.FindObjectsByType<CareStation>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(b => b.IsFree);
            if (freeBed == null) {
                return _state = BTNodeState.Failure;
            }

            freeBed.SetSettler(_settler);
            return _state = BTNodeState.Success;
        }
    }
}