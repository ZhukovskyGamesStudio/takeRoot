using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AI.Node.Jobs {
    public class Action_FindDinamoMachineToCharge : BTNode {
        private readonly Settler _settler;
        private readonly Func<DinamoMachine, bool> _condition;

        public Action_FindDinamoMachineToCharge(Settler settler) {
            _settler = settler;
            _condition = machine => machine.LowElectricity && machine.IsFree;
        }

        public override BTNodeState Evaluate() {
            DinamoMachine dichargedDinamoMachine =
                Object.FindObjectsByType<DinamoMachine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(_condition);
            if (dichargedDinamoMachine == null) {
                return _state = BTNodeState.Failure;
            }

            dichargedDinamoMachine.DinamoCharger = _settler.DinamoCharger;
            _settler.Data.targets.DinamoMachine = dichargedDinamoMachine;
            _settler.Data.curMovePos = dichargedDinamoMachine.GetComponent<CommandTarget>().InteractPosition.position;
            return _state = BTNodeState.Success;
        }
    }
}