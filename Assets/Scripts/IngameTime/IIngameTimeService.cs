using UniRx;

public interface IIngameTimeService : IService {

    public IngameTimeData IngameTimeData { get; set; }
    
}
