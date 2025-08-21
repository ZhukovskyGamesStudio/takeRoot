using UnityEngine;

public class GameEventsManager : MonoBehaviour, IInitableInstance
{
   
    public WorldObjectsEvents WorldObjectsEvents;
    
    public void Init()
    {
        ObsoleteCoreEntryPoint.GameEventsManager = this;
        
        WorldObjectsEvents = new WorldObjectsEvents();
    }

}