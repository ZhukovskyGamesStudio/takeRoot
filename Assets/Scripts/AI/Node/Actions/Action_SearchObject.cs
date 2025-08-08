using UnityEngine;

namespace AI.Node.Jobs {
	public class Action_SearchObject : BTNode {
		private Settler _settler;
		private float _lastSearchTime;

		public Action_SearchObject(Settler settler) {
			_settler = settler;
		}
		
		public override BTNodeState Evaluate() {
			var searchable = _settler.Data.currTarget;
			if (searchable.Searched) {
				_settler.Searcher.Cancel();
				searchable.EndSearch();
				return BTNodeState.Success;
			}
			_settler.Searcher.Search(searchable);
			return BTNodeState.Running;
		}
	}
}
