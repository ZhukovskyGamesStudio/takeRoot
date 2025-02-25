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

    public event Action<Settler> onSettlerModeChanged;
    public void OnSettlerModeChanged(Settler settler)
    {
        onSettlerModeChanged?.Invoke(settler);
    }

    public event Action onSettlersMerged;

    public void OnSettlersMerged()
    {
        onSettlersMerged?.Invoke();
    }

}