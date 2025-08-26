using System;

namespace AI.Node.Conditions {
    public class IsTacticalMode : Conditional {
        public IsTacticalMode(Func<bool> condition) : base(condition) { }
    }
}