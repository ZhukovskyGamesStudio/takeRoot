using UnityEngine;

public class GameEventsManager : MonoBehaviour, IInitableInstance
{
   
    public WorldObjectsEvents WorldObjectsEvents;
    
    public void Init()
    {
        Core.GameEventsManager = this;
        
        WorldObjectsEvents = new WorldObjectsEvents();
    }

}