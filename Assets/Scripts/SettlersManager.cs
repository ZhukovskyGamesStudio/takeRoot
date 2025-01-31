using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersManager : MonoBehaviour, IInitableInstance {
    public static SettlersManager Instance => Core.SettlersManager;

    public List<Settler> Settlers => FindObjectsByType<Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

    public void Init() {
        Core.SettlersManager = this;
    }
}