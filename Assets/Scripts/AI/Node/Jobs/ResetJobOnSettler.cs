namespace AI.Node.Jobs {
    public class ResetJobOnSettler : BTNode {
        private Settler _settler;

        public ResetJobOnSettler(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            CommandTarget target = _settler.Data.currTarget;
            if (target != null) {
                target.Data.AssignedSettler = null;
            }

            _settler.Data.currJob = JobType.None;
            _settler.Data.currTarget = null;
            _settler.Searcher.Cancel();
            _settler.Destroyer.Cancel();
            _settler.Waterer.Cancel();
            return BTNodeState.Failure;
        }
    }
}