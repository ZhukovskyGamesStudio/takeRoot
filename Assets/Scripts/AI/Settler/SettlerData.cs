using System;
using Unity.Netcode;
using UnityEngine;

namespace AI {
    [Serializable]
    public class SettlerData : INetworkSerializable, IEquatable<AI.SettlerData> {
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

        [Header("Names")]
        public Settler_Names names;

        [Header("Needs")]
        public NetworkVariable<Settler_Needs> needs = new();

        public float needsUpdateCooldown;
        public float needsUpdateTimer;

        [Header("Transport for crafting")]
        public Settler_TransportForCrafting craftingTransport;

        [HideInInspector]
        public Settler_Targets targets;

        [Header("Transport for Building")]
        public Settler_TransportForBuilding buildingTransport;

        [Header("Idle move")]
        public float IdleMoveCooldown;
        [HideInInspector]
        public float IdleMoveTimer;
        public bool IsIdle;

        [Header("Tactical")]
        public Settler_Tactical tactical;

        [Header("Hit")]
        public float HitTime = 1.3f;

        public void Init() {
            needs.Value.CareData.careChange = needs.Value.CareData.defaultCareChange;
            needs.Value.Energy.energyChange = needs.Value.Energy.defaultEnergyChange;
            needs.Value.Energy.currentEnergy = needs.Value.Energy.maxEnergy;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref curMovePos);
        }

        public bool Equals(SettlerData other) {
            if (other is null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return Dead == other.Dead && currJob == other.currJob && Equals(currTarget, other.currTarget) &&
                   Equals(subsequentTarget, other.subsequentTarget) && HasMovePos == other.HasMovePos && curMovePos.Equals(other.curMovePos) &&
                   Equals(ItemInHands, other.ItemInHands) && Condition == other.Condition && Equals(names, other.names) &&
                   Equals(needs, other.needs) && needsUpdateCooldown.Equals(other.needsUpdateCooldown) &&
                   needsUpdateTimer.Equals(other.needsUpdateTimer) && Equals(craftingTransport, other.craftingTransport) &&
                   Equals(targets, other.targets) && Equals(buildingTransport, other.buildingTransport) &&
                   IdleMoveCooldown.Equals(other.IdleMoveCooldown) && IdleMoveTimer.Equals(other.IdleMoveTimer) && IsIdle == other.IsIdle &&
                   Equals(tactical, other.tactical) && HitTime.Equals(other.HitTime);
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != GetType()) {
                return false;
            }

            return Equals((SettlerData)obj);
        }

        public override int GetHashCode() {
            HashCode hashCode = new HashCode();
            hashCode.Add(Dead);
            hashCode.Add((int)currJob);
            hashCode.Add(currTarget);
            hashCode.Add(subsequentTarget);
            hashCode.Add(HasMovePos);
            hashCode.Add(curMovePos);
            hashCode.Add(ItemInHands);
            hashCode.Add((int)Condition);
            hashCode.Add(names);
            hashCode.Add(needs);
            hashCode.Add(needsUpdateCooldown);
            hashCode.Add(needsUpdateTimer);
            hashCode.Add(craftingTransport);
            hashCode.Add(targets);
            hashCode.Add(buildingTransport);
            hashCode.Add(IdleMoveCooldown);
            hashCode.Add(IdleMoveTimer);
            hashCode.Add(IsIdle);
            hashCode.Add(tactical);
            hashCode.Add(HitTime);
            return hashCode.ToHashCode();
        }
    }

    public enum SettlerCondition {
        Neutral,
        Sleep,
        Breakdown,
        Inspiration
    }
}