using UniRx;

public class RaceService : IRaceService {
    public RaceService(Race race) {
        RaceReactive.Value = race;
    }

    public ReactiveProperty<Race> RaceReactive { get; set; } = new ReactiveProperty<Race>();
}