
    using System;

    public class PowerProvider : ECSComponent
    {
        public override int GetDependancyPriority()
        {
           return 0;
        }

        public override void Init(ECSEntity entity)
        {
        }
    }
