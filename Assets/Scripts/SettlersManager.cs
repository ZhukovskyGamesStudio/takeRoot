using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersManager : MonoBehaviour {
    public static SettlersManager Instance;

    private void Awake() {
        Instance = this;
    }

    public List<Settler> Settlers => FindObjectsByType<Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
}