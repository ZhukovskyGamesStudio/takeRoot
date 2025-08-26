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
        public IResourceCarrier ResourceCarrier;
        public WorkerAnimator WorkerAnimator { get; private set; }

        private void Start() {
            WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
            Mover = GetComponent<IMovable>();
            Searcher = GetComponent<ISearcher>();
            Destroyer = GetComponent<IDestroyer>();
            Waterer = GetComponent<IWaterer>();
            ResourceCarrier = GetComponent<IResourceCarrier>();
            Searcher.Init(WorkerAnimator);
            Destroyer.Init(WorkerAnimator);
            Waterer.Init(WorkerAnimator);
            _root = CreateBT();
            _stateBt = new Sequence().AddChild(new Action_HandleEnergy(this));
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
            ICommandService commands = ServiceLocator.Container.Single<ICommandService>();
            ICraftingService crafting = ServiceLocator.Container.Single<ICraftingService>();
            IResourceManager resources = ServiceLocator.Container.Single<IResourceManager>();
            BTRoot_Settler root = new(this, commands, crafting, resources);
            return root;
        }
    }
}