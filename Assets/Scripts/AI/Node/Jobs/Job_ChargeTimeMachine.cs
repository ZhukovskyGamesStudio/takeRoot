namespace AI.Node.Jobs {
    public class Job_ChargeTimeMachine : Sequence {
        public Job_ChargeTimeMachine(Settler settler, ITimeScaleService timeScaleService) {
            AddChild(new Action_FindTimeMachine(settler, timeScaleService));
            
            AddChild(new ConditionalAction()
                .Do(new Action_MoveToPos(settler))
                .While(() => settler.Data.targets.TimeMachine != null));

            AddChild(new Action_ChargeTimeMachine(settler));
        }
    }
}