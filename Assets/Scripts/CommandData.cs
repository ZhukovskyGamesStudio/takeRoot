using System;
using System.Collections.Generic;

[Serializable]
public class CommandData {
    public Command CommandType;
    public Interactable Interactable;
    public Interactable Additional;
    public Settler Settler;
    public PlannedCommandView PlannedCommandView;

    public List<Settler> UnablePerformSettlers = new List<Settler>();

    //TODO refactor
    public object AdditionalData;
    public Action TriggerCancel;

    public bool HasSubsequentCommand => CommandType is Command.Transport or Command.Break or Command.GatherResources or 
        Command.Build or Command.GatherResourcesForCraft or Command.Craft or Command.PrepareToCraft;
}