using AI.Node;
using AI.Node.Jobs;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Profiling;

namespace AI {
	public class Settler : MonoBehaviour {
		private BTNode _root;
		private BTNode _stateBt;
		public SettlerData Data;
		
		public IMovable Mover;
		public ISearcher Searcher;
		public IDestroyer Destroyer;
		public IWaterer Waterer;
		public WorkerAnimator WorkerAnimator { get; private set; }


		void Start() {
			WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
			Mover = GetComponent<IMovable>();
			Searcher = GetComponent<ISearcher>();
			Destroyer = GetComponent<IDestroyer>();
			Waterer = GetComponent<IWaterer>();
			Searcher.Init(WorkerAnimator);
			Destroyer.Init(WorkerAnimator);
			Waterer.Init(WorkerAnimator);
			_root = CreateBT();
			_stateBt = new Sequence()
				.AddChild(new Action_HandleEnergy(this));
		}

		private void Update() {
			Profiler.BeginSample("Evaluate Settler Action BT");
			_root?.Evaluate();
			Profiler.EndSample();
			Profiler.BeginSample("Evaluate Settler State change BT");
			_stateBt?.Evaluate();
			Profiler.EndSample();
		}

		public void Sleep() {
			Data.energy.isSleeping = true;
			WorkerAnimator.PlaySleep();
		}

		public void WakeUp() {
			Data.energy.isSleeping = false;
			WorkerAnimator.ResetToIdle();
		}
		
		private BTNode CreateBT() {
			var commands = ServiceLocator.Container.Single<ICommandService>();
			var crafting = ServiceLocator.Container.Single<ICraftingService>();
			var resources = ServiceLocator.Container.Single<IResourceManager>();
			var root = new BTRoot_Settler(this, commands, crafting, resources);
			return root;
		}
	}
}