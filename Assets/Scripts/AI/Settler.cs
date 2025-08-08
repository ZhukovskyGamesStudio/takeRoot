using System;
using AI.Node;
using AI.Node.Jobs;
using UnityEngine;

namespace AI {
	public class Settler : MonoBehaviour {
		private BTNode _root;
		public SettlerData Data;
		
		
		public IMovable Mover;
		public ISearcher Searcher;
		public WorkerAnimator WorkerAnimator { get; private set; }


		void Start() {
			WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
			Data = new SettlerData();
			Mover = GetComponent<IMovable>();
			Searcher = GetComponent<ISearcher>();
			Searcher.Init(WorkerAnimator);

			_root = CreateBT();
		}

		private void Update() {
			_root?.Evaluate();
		}

		private BTNode CreateBT() {
			var root = new Jobs(this);
			return root;
		}
	}
}