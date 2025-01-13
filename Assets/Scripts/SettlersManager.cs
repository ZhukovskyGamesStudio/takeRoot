using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersManager : MonoBehaviour {
    public static SettlersManager Instance;

    private void Awake() {
        Instance = this;
    }

    public List<Chamomile> Settlers => FindObjectsByType<Chamomile>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
}