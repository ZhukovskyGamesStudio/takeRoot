using UnityEngine;

public abstract class ECSComponent : MonoBehaviour {
    public abstract int GetDependancyPriority();
    public abstract void Init(ECSEntity entity);
}