using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersService : ISettlersService {
    public List<AI.Settler> MySettlers(Race myRace) {
        //TODO refator withor find
        var res = Object.FindObjectsByType<AI.Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Where(s => s.Data.names.Race == myRace);
        return res.ToList();
    }
}