namespace AI.Node.Jobs {
    public class Action_SearchObject : BTNode {
        private Settler _settler;

        public Action_SearchObject(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            CommandTarget searchable = _settler.Data.currTarget;
            if (searchable.Searched) {
                _settler.Searcher.Cancel();
                _settler.Data.currTarget.CancelJob();
                _settler.Data.currJob = JobType.None;
                _settler.Data.currTarget = null;
                searchable.EndSearch();
                return _state = BTNodeState.Success;
            }

            _settler.Searcher.Search(searchable);
            return _state = BTNodeState.Running;
        }
    }
}