using UnityEngine;

public class GameEventsManager : MonoBehaviour, IInitableInstance
{
    public static GameEventsManager Instance;
    public WorldObjectsEvents WorldObjectsEvents;
    
    public void Init()
    {
        Instance = this;
        
        WorldObjectsEvents = new WorldObjectsEvents();
    }

}