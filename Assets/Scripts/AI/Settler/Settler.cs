using AI.Node;
using AI.Node.Jobs;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Profiling;

namespace AI {
    public class Settler : MonoBehaviour {
        
        public static bool GlobalGodmode;
        private BTRoot_Settler _root;
        private BTNode _stateBt;
        public SettlerData Data;

        public IMovable Mover;
        public ISearcher Searcher;
        public IPlanter Farmer;
        public ICareGiver CareGiver;
        public IDestroyer Destroyer;
        public IWaterer Waterer;
        public IResourceCarrier ResourceCarrier;
        public ICrafter Crafter;
        public IBuilder Builder;
        public ITimeMachineCharger TimeMachineCharger;
        
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
            CareGiver = GetComponent<ICareGiver>();
            Builder = GetComponent<IBuilder>();
            TimeMachineCharger = GetComponent<ITimeMachineCharger>();
            
            Searcher.Init(WorkerAnimator);
            Destroyer.Init(WorkerAnimator);
            Waterer.Init(WorkerAnimator);
            Crafter.Init(WorkerAnimator);
            Builder.Init(WorkerAnimator);
            Farmer.Init(WorkerAnimator);
            CareGiver.Init(WorkerAnimator);
            TimeMachineCharger.Init(WorkerAnimator);
            
            _root = CreateRootBt();
            _stateBt = CreateStateBt();
            Data.Init();
        }

        private void Update() {
            Profiler.BeginSample("Evaluate Settler Action BT");
            _root?.Evaluate();
            Profiler.EndSample();
            Profiler.BeginSample("Evaluate Settler State change BT");
            _stateBt?.Evaluate();
            Profiler.EndSample();
        }
        
        public void SetMood(Mood mood) => WorkerAnimator.SetMood(mood);

        public void SetTactical(bool isTactical) {
            Data.tactical.IsTactical = isTactical;
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
            if (GlobalGodmode) return;

            gameObject.SetActive(false);
            Data.Dead = true;
        }

        public void Sleep() {
            Data.needs.Energy.isSleeping = true;
            WorkerAnimator.PlaySleep();
        }

        public void WakeUp() {
            Data.needs.Energy.isSleeping = false;
            WorkerAnimator.ResetToIdle();
        }
        
        public void StartReceiveCare() {
            Data.needs.CareData.isTakingCareOf = true;
            WorkerAnimator.PlaySleep();
        }

        public void StopReceivingCare() {
            Data.needs.CareData.isTakingCareOf = false;
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
            ITimeScaleService timeScale = ServiceLocator.Container.Single<ITimeScaleService>();
            BTRoot_Settler root = new(this, commands, crafting, resources, building, farming, timeScale);
            return root;
        }
    }
}