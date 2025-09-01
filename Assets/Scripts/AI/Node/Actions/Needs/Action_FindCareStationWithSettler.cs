using System.Linq;
using UnityEngine;

namespace AI.Node.Jobs {
    public class Action_FindCareStationWithSettler : BTNode {
        private readonly Settler _settler;

        public Action_FindCareStationWithSettler(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            var race = _settler.Data.names.Race;
            CareStation stationWithSettler = Object.FindObjectsByType<CareStation>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .FirstOrDefault(b => b.CareSettlerReady&& !b.IsFree && b.IsFreeCaregiver && b.SettlerRace != race );

            if (stationWithSettler == null) {
                return _state = BTNodeState.Failure;
            }

            _settler.Data.targets.CareStation = stationWithSettler;
            stationWithSettler.SetCaregiverSettler(_settler);
            _settler.Data.curMovePos = stationWithSettler.CaregiverPos.position;
            return _state = BTNodeState.Success;
        }
    }
}