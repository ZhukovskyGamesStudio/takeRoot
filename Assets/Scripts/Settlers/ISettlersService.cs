using System.Collections.Generic;
using UnityEngine;

public interface ISettlersService : IService {
    public AI.Settler GetSettlerInArea(Rect area);
    public List<AI.Settler> MySettlers(Race myRace);
    public List<AI.Settler> AllSettlers();
}