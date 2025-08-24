using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI {
    [Serializable]
    public class SettlerData {
        public bool IsTactical;

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

        [Header("Names")]
        public Settler_Names names;

        [Header("Energy")]
        public Settler_EnergyData energy;

        [Header("Transport for crafting")]
        public Settler_TransportForCrafting craftingTransport;

        [Header("Idle move")]
        public float IdleMoveCooldown;

        public float IdleMoveTimer;

        [Header("Hit")]
        public float HitTime = 1.3f;
    }

    public enum SettlerCondition {
        Neutral,
        Sleep,
        Breakdown,
        Inspiration
    }
}