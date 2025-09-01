using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class SettlerData {
        public bool Dead;

        [Header("Jobs")]
        public JobType currJob;

        public CommandTarget currTarget;
        public CommandTarget subsequentTarget;
        public bool HasMovePos;
        public Vector3 curMovePos;

        public bool HasJob => currJob != JobType.None;
        public bool CanDoJob => true; //TODO: make condition

        public CommandTarget ItemInHands;
        public bool HasItem => ItemInHands != null;

        public SettlerCondition Condition;

        [Header("Needs")]
        public Settler_Needs needs;
        public float needsUpdateCooldown;
        public float needsUpdateTimer;

        [Header("Names")]
        public Settler_Names names;

        [Header("Energy")]
       

        [Header("Transport for crafting")]
        public Settler_TransportForCrafting craftingTransport;

        public Settler_Targets targets;
        
        [Header("Transport for Building")]
        public Settler_TransportForBuilding buildingTransport;
        
        [Header("Idle move")]
        public float IdleMoveCooldown;
        public float IdleMoveTimer;
        public bool IsIdle;
        
        [Header("Tactical")]
        public Settler_Tactical tactical;

        [Header("Hit")]
        public float HitTime = 1.3f;


        public void Init() {
            needs.CareData.careChange = needs.CareData.defaultCareChange;
            needs.Energy.energyChange = needs.Energy.defaultEnergyChange;
        }
    }

    public enum SettlerCondition {
        Neutral,
        Sleep,
        Breakdown,
        Inspiration
    }
}