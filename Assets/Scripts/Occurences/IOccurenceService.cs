using System;

public interface IOccurenceService : IService {
    public Action<OccurenceConfig> OnOccurenceSpawn { get; set; }
}
