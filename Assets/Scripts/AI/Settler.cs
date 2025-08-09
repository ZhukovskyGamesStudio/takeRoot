using System;
using System.ComponentModel.Design;
using AI.Node;
using AI.Node.Jobs;
using CodeBase.Services;
using UnityEngine;

namespace AI {
	public class Settler : MonoBehaviour {
		private BTNode _root;
		public SettlerData Data;
		
		
		public IMovable Mover;
		public ISearcher Searcher;
		public IDestroyer Destroyer;
		public WorkerAnimator WorkerAnimator { get; private set; }


		void Start() {
			WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
			Data = new SettlerData();
			Mover = GetComponent<IMovable>();
			Searcher = GetComponent<ISearcher>();
			Destroyer = GetComponent<IDestroyer>();
			Searcher.Init(WorkerAnimator);
			Destroyer.Init(WorkerAnimator);

			_root = CreateBT();
		}

		private void Update() {
			_root?.Evaluate();
		}

		private BTNode CreateBT() {
			var commands = ServiceLocator.Container.Single<ICommandService>();
			var root = new Jobs(this, commands);
			return root;
		}
	}
}