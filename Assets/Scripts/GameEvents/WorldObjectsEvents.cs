using System;

public class WorldObjectsEvents
{
    public event Action<string> onDestroyed;
    public void OnDestroyed(string id)
    {
        onDestroyed?.Invoke(id);
    }
}