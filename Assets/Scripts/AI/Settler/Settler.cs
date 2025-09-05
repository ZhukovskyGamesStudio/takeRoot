using AI.Node;
using AI.Node.Jobs;
using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Profiling;

namespace AI {
    public class Settler : NetworkBehaviour {
        [SerializeField]
        private Gravestone _gravestonePrefab;
        
        public static bool GlobalGodmode;
        public static bool Immortal;
        private BTRoot_Settler _root;
        private BTNode _stateBt;
        public NetworkVariable<SettlerData> NetworkData;
        public SettlerData Data => NetworkData.Value;

        public Race Race => Data.names.Race;

        public IMovable Mover;
        public ISearcher Searcher;
        public IPlanter Farmer;
        public IDinamoCharger DinamoCharger;
        public ICareGiver CareGiver;
        public IDestroyer Destroyer;
        public IWaterer Waterer;
        public IResourceCarrier ResourceCarrier;
        public ICrafter Crafter;
        public IBuilder Builder;
        public ITimeMachineCharger TimeMachineCharger;
        public IAttacker Attacker;
        
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
            DinamoCharger = GetComponent<IDinamoCharger>();
            TimeMachineCharger = GetComponent<ITimeMachineCharger>();
            Attacker = GetComponent<IAttacker>();
            
            Mover.Init(WorkerAnimator);
            Searcher.Init(WorkerAnimator);
            Destroyer.Init(WorkerAnimator);
            Waterer.Init(WorkerAnimator);
            Crafter.Init(WorkerAnimator);
            Builder.Init(WorkerAnimator);
            Farmer.Init(WorkerAnimator);
            CareGiver.Init(WorkerAnimator);
            DinamoCharger.Init(WorkerAnimator);
            TimeMachineCharger.Init(WorkerAnimator);
            Attacker.Init(WorkerAnimator);
            
            Data.Init();

            if (IsOwner || AdminManager.IsFakeOnline) {
                _root = CreateRootBt();
                _stateBt = CreateStateBt();
            }
        }

        private void Update() {
            
            if (!IsOwner && !AdminManager.IsFakeOnline) {
                return;
            }
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
            Data.needs.Value.StressData.breakdownTimer = 0;
            Data.Condition = SettlerCondition.Breakdown;
        }

        public void EndBreakdown() {
            Data.Condition = SettlerCondition.Neutral;
            Data.needs.Value.StressData.currentStress = Data.needs.Value.StressData.stressAfterBreakdown;
        }

        public void Die(DeathCause cause) {
            if (GlobalGodmode) {
                return;
            }


            SetAsDead(cause);
            DieClientRpc(cause);
            SpawnTombstone(cause);
        }
        
        [ClientRpc]
        private void DieClientRpc(DeathCause cause) {
            SetAsDead(cause);
        }

        private void SetAsDead(DeathCause cause) {
            gameObject.SetActive(false);
            Data.Dead = true;
           
        }

        private void SpawnTombstone(DeathCause cause) {
            Vector2 gravePos = new (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            Gravestone gravestone = ServiceLocator.Container.Single<INetworkService>().InstantiateAndSpawn(_gravestonePrefab,gravePos);
            gravestone.SetData(Data.names.Name, cause);
        }

        public void TeleportToPos(Vector3 pos) {
            transform.position = pos;
            TeleportToPosClientRpc(pos);
        }

        [ClientRpc]
        private void TeleportToPosClientRpc(Vector3 pos) {
            transform.position = pos;
        }

        public void Sleep() {
            Data.needs.Value.Energy.isSleeping = true;
            WorkerAnimator.PlaySleep();
        }

        public void WakeUp() {
            Data.needs.Value.Energy.isSleeping = false;
            WorkerAnimator.ResetToIdle();
        }
        
        public void StartInteract() {
            WorkerAnimator.PlaySearch();
        }
        
        public void StopInteract() {
            Data.needs.Value.CareData.isTakingCareOf = true;
            WorkerAnimator.ResetToIdle();
        }
        
        public void StartReceiveCare() {
            Data.needs.Value.CareData.isTakingCareOf = true;
            WorkerAnimator.PlaySleep();
        }

        public void StopReceivingCare() {
            Data.needs.Value.CareData.isTakingCareOf = false;
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
        
        //TODO refactor this
        public Vector2Int PosOnGrid => new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        [ClientRpc]
        public void UpdateNamesDataClientRpc(string settlerName) {
            Data.names.Name = settlerName;
        }
        
        [ClientRpc]
        public void UpdateNeedsClientRpc(float hp,Settler_EnergyData energy, Settler_SatietyData satiety, Settler_CareData care, Settler_StressData stress) {
            Data.needs.Value.Hp = hp;
            Data.needs.Value.Energy = energy;
            Data.needs.Value.SatietyData = satiety;
            Data.needs.Value.CareData = care;
            Data.needs.Value.StressData = stress;
        }
    }

    public enum DeathCause {
        Unknown,
        Hunger,
        Care,
        Hp
    }
}