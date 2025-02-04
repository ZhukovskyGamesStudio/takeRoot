public class QuestStepStatus
{
    public string Status;
    public QuestStepState State;
    public Race Race;

    public QuestStepStatus(QuestStepState state, Race race, string status = "")
    {
        Status = status;
        State = state;
        Race = race;
    }
}

public enum QuestStepState
{
    Active,
    Finished
}