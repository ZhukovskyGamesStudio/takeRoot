using System;

namespace AI.Node.Jobs {
    public class Job_ChargeDinamoMachine : Sequence {
        public Job_ChargeDinamoMachine(Settler settler) {
            AddChild(new Action_FindDinamoMachineToCharge(settler));

            SettlerData data = settler.Data;
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler))
                .While(() => data.targets.DinamoMachine != null && data.targets.DinamoMachine.LowElectricity);

            Selector moveOrCancel = new Selector()
                .AddChild(move)
                .AddChild(new Action_ClearDinamoMachine(settler));

            AddChild(moveOrCancel);
            AddChild(new Action_ChargeDinamoMachine(settler));
        }
    }
}