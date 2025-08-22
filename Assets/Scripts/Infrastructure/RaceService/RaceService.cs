using UniRx;

public class RaceService : IRaceService {
    public RaceService(Race race) {
        RaceRactive.Value = race;
    }

    public ReactiveProperty<Race> RaceRactive { get; set; } = new ReactiveProperty<Race>();
}