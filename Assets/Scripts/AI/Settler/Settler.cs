using AI.Node;
using AI.Node.Jobs;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Profiling;

namespace AI {
    public class Settler : MonoBehaviour {
        private BTRoot_Settler _root;
        private BTNode _stateBt;
        public SettlerData Data;

        public IMovable Mover;
        public ISearcher Searcher;
        public IPlanter Farmer;
        public IDestroyer Destroyer;
        public IWaterer Waterer;
        public IResourceCarrier ResourceCarrier;
        public ICrafter Crafter;
        public IBuilder Builder;
        public WorkerAnimator WorkerAnimator { get; private set; }

        private void Start() {
            WorkerAnimator = GetComponentInChildren<WorkerAnimator>();
            Mover = GetComponent<IMovable>();
            Searcher = GetComponent<ISearcher>();
            Farmer = GetComponent<IPlanter>();
            Destroyer = GetComponent<IDestroyer>();
            Waterer = GetComponent<IWaterer>();
            ResourceCarrier = GetComponent<IResourceCarrier>();
            Crafter = GetComponent<ICrafter>();
            Builder = GetComponent<IBuilder>();
            Searcher.Init(WorkerAnimator);
            Destroyer.Init(WorkerAnimator);
            Waterer.Init(WorkerAnimator);
            Crafter.Init(WorkerAnimator);
            Builder.Init(WorkerAnimator);
            Farmer.Init(WorkerAnimator);
            _root = CreateRootBt();
            _stateBt = CreateStateBt();
        }

        private void Update() {
            Profiler.BeginSample("Evaluate Settler Action BT");
            _root?.Evaluate();
            Profiler.EndSample();
            Profiler.BeginSample("Evaluate Settler State change BT");
            _stateBt?.Evaluate();
            Profiler.EndSample();
        }

        public void SetTactical(bool isTactical) {
            Data.tactical.IsTactical = isTactical;
            _root.Reset();
        }
        public void StartBreakdown() {
            Data.needs.StressData.breakdownTimer = 0;
            Data.Condition = SettlerCondition.Breakdown;
        }

        public void EndBreakdown() {
            Data.Condition = SettlerCondition.Neutral;
            Data.needs.StressData.currentStress = Data.needs.StressData.stressAfterBreakdown;
        }

        public void Die() {
            gameObject.SetActive(false);
            Data.Dead = true;;
        }

        public void Sleep() {
            Data.energy.isSleeping = true;
            WorkerAnimator.PlaySleep();
        }

        public void WakeUp() {
            Data.energy.isSleeping = false;
            WorkerAnimator.ResetToIdle();
        }

        private BTNode CreateStateBt() {
            BTNode stateBt = new Sequence()
                .AddChild(new Action_HandleNeedsChange(this));
            
            
            return stateBt;
        }

        private BTRoot_Settler CreateRootBt() {
            ICommandService commands = ServiceLocator.Container.Single<ICommandService>();
            ICraftingService crafting = ServiceLocator.Container.Single<ICraftingService>();
            IBuildingService building = ServiceLocator.Container.Single<IBuildingService>();
            IResourceManager resources = ServiceLocator.Container.Single<IResourceManager>();
            IFarmingService farming = ServiceLocator.Container.Single<IFarmingService>();
            BTRoot_Settler root = new(this, commands, crafting, resources, building,farming);
            return root;
        }
    }
}