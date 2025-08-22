using UniRx;

public interface IRaceService : IService {
    public ReactiveProperty<Race> RaceReactive { get; set; }
}
