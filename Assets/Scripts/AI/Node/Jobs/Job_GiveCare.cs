using System;

namespace AI.Node.Jobs {
    public class Job_GiveCare : Sequence {
        public Job_GiveCare(Settler settler) {
            AddChild(new Action_FindCareStationWithSettler(settler));

            SettlerData data = settler.Data;
            Func<bool> condition = () => data.targets.CareStation && !data.targets.CareStation.IsFree;
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);

            Selector moveOrCancel = new Selector().AddChild(move).AddChild(new Action_ClearCareStationFromCaregiver(settler));

            AddChild(moveOrCancel);
            AddChild(new Action_GiveCare(settler));
        }
    }
}