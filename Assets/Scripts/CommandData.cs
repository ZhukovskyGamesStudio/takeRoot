using System;
using UnityEngine;

[Serializable]
public class CommandData {
    public Command CommandType;
    public InteractableObject InteractableObject;
    public InteractableObject AdditionalObject;
    public Chamomile Settler;
    public PlannedCommandView PlannedCommandView;
    public Action TriggerCancel;

    public bool HasSubsequentCommand => CommandType == Command.Transport;
}
