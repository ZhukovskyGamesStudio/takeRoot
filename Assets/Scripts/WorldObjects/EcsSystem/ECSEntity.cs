using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ECSEntity : MonoBehaviour {
    private List<ECSComponent> _components;

    protected virtual void Awake() {
        _components = GetComponents<ECSComponent>().ToList();
        foreach (ECSComponent component in _components) {
            component.Init(this);
        }
    }

    public T GetEcsComponent<T>() where T : ECSComponent {
        return _components.FirstOrDefault(c => c is T) as T;
    }
}