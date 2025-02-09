using System;
using System.Collections.Generic;

[Serializable]
public class CommandData {
    public Command CommandType;
    public Interactable Interactable;
    public Interactable Additional;
    public Settler Settler;
    public PlannedCommandView PlannedCommandView;
    public Action TriggerCancel;
    public List<Settler> UnablePerformSettlers = new List<Settler>();

    public bool HasSubsequentCommand => CommandType is Command.Transport or Command.Break or Command.GatherResources or Command.Build;
}
