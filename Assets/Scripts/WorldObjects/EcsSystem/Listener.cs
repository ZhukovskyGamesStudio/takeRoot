using System;
using UnityEngine;

namespace WorldObjects
{
    public class Listener : ECSComponent
    {
        public Action<Vector2Int> HasHeard;
        public Gridable Gridable {get; private set;}
        
        public override int GetDependancyPriority()
        {
            return 0;
        }
        public override void Init(ECSEntity entity)
        {
            Gridable = entity.GetEcsComponent<Gridable>();
        }
    }
}