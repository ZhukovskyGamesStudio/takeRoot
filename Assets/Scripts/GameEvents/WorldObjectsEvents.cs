using System;

public class WorldObjectsEvents
{
    public event Action<string> onDestroyed;
    public void OnDestroyed(string id)
    {
        onDestroyed?.Invoke(id);
    }
    
    public event Action<string> onDied;
    public void OnDied(string id)
    {
        onDied?.Invoke(id);
    }
    
    
}