using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersService : ISettlersService {
    private List<AI.Settler> settlers = new List<AI.Settler>();
    public SettlersService() {
        settlers = Object.FindObjectsOfType<AI.Settler>().ToList();
    }
    public AI.Settler GetSettlerInArea(Rect area) {
        return settlers.FirstOrDefault(s => area.Contains(s.transform.position));
    }

    public List<AI.Settler> MySettlers(Race myRace) {
        //TODO refator withor find
        IEnumerable<AI.Settler> res = Object.FindObjectsByType<AI.Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(s => s.Data.names.Race == myRace);
        return res.ToList();
    }
}