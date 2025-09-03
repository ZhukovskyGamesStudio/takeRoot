using UniRx;

public interface INetworkService : IService {
    
    public NetworkDataHolder NetworkDataHolder  { get; }
    
    public ReactiveProperty<Race> MyRace { get; set; }
    
}