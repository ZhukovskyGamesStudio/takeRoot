using System.Collections.Generic;

public interface ISettlersService : IService {
    public List<AI.Settler> MySettlers(Race myRace);
}
