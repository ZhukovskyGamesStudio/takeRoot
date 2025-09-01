using System;
using UnityEngine;

namespace AI {
    [Serializable]
    public class Settler_Names {
        [HideInInspector]
        public string Name;
        public Race Race;
        public Subrace Subrace;
    }
}