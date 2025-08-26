using System.Collections.Generic;

public class CommandService : ICommandService {
    public Dictionary<int, CommandTarget> JobTargets { get; } = new();

    public void HandleCommandRequest(CommandType type, bool withSelectedSettler) {
        switch (type) {
            case CommandType.Search:
                break;
            case CommandType.Destroy:
                break;
            case CommandType.Water:
                break;
        }
    }

    public CommandTarget GetJob() {
        foreach (KeyValuePair<int, CommandTarget> kvp in JobTargets) {
            CommandTarget jobTarget = kvp.Value;

            if (!jobTarget.Data.Reserved) {
                return jobTarget;
            }
        }

        return null;
    }

    public void RegisterJob(int id, CommandTarget target) {
        JobTargets.Add(id, target);
    }

    public void UnregisterJob(int targetId) {
        JobTargets.Remove(targetId);
    }
}