using UniRx;

public interface IRaceService : IService {
    public ReactiveProperty<Race> RaceRactive { get; set; }
}
