using UniRx;

public class RaceService : IRaceService {
    private readonly INetworkService _networkService;

    public RaceService(INetworkService networkService) {
        _networkService = networkService;
        RaceReactive.Value = _networkService.MyRace.Value;
    }

    public ReactiveProperty<Race> RaceReactive { get; set; } = new();
}