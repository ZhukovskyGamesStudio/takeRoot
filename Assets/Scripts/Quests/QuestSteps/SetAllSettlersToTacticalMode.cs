using System.Linq;
using UnityEngine;

public class SetAllSettlersToTacticalMode : QuestStep
{
    private int _settlersCount;
    private int _settlersInTacticalMode;
    private void Start()
    {
        _settlersCount = Core.SettlersManager.MySettlers.Count();
        foreach (SettlerData settler in Core.SettlersManager.MySettlers)
        {
            if (settler._mode == Mode.Tactical)
                _settlersInTacticalMode++;
        }
        UpdateQuestStepStatus(_status + _settlersInTacticalMode + "/" + _settlersCount);
        if (_settlersInTacticalMode == _settlersCount)
        {
            FinishStep();
            // TODO: Add Merge toggle to command panel
            return;
        }
        Core.GameEventsManager.WorldObjectsEvents.onSettlerModeChanged += OnStatusChanged;
    }
    private void OnStatusChanged(Settler settler)
    {
        if (IsFinished)
            return;
        if (Core.SettlersManager.MySettlers.Contains(settler.SettlerData) && settler.Mode == Mode.Tactical)
            _settlersInTacticalMode++;
        if (Core.SettlersManager.MySettlers.Contains(settler.SettlerData) && settler.Mode == Mode.Planning)
            _settlersInTacticalMode--;
        
        UpdateQuestStepStatus(_status + _settlersInTacticalMode + "/" + _settlersCount);
        
        if (_settlersInTacticalMode == _settlersCount)
        {
            FinishStep();
            // TODO: Add Merge toggle to command panel
        }
    }
}