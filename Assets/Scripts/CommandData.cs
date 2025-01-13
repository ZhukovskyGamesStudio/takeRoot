using System;

[Serializable]
public class CommandData {
    public Command CommandType;
    public Interactable Interactable;
    public Interactable Additional;
    public Chamomile Settler;
    public PlannedCommandView PlannedCommandView;
    public Action TriggerCancel;

    public bool HasSubsequentCommand => CommandType is Command.Transport or Command.Attack;
}
