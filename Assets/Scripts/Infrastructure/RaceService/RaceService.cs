public class RaceService : IRaceService {
    private Race _race;

    public RaceService(Race race) {
        _race = race;
    }

    public Race MyRace() {
        return _race;
    }

    public void SetRace(Race race) {
        _race = race;
    }
}