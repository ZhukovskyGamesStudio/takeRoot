namespace AI.Node.Jobs {
    public class Action_FindStorage : BTNode {
        private readonly Settler _settler;

        public Action_FindStorage(Settler settler) {
            _settler = settler;
        }

        public override BTNodeState Evaluate() {
            throw new System.NotImplementedException();
        }
    }
}