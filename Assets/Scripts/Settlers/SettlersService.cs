using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersService : ISettlersService {
    private List<AI.Settler> _settlers = new List<AI.Settler>();
    public SettlersService() {
        _settlers = Object.FindObjectsOfType<AI.Settler>().ToList();
    }
    public AI.Settler GetSettlerInArea(Rect area) {
        return _settlers.FirstOrDefault(s => area.Contains(s.transform.position));
    }

    public List<AI.Settler> MySettlers(Race myRace) {
        //TODO refator withor find
        IEnumerable<AI.Settler> res = Object.FindObjectsByType<AI.Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(s => s.Data.names.Race == myRace);
        return res.ToList();
    }

    public List<AI.Settler> AllSettlers() {
        return _settlers;
    }
}