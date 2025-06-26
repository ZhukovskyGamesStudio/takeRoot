using UnityEngine;

public class CommandPerformer : MonoBehaviour {
	public CommandType CommandCapabilities { get; private set; }
	public int CurrentCommand { get; set; }
	public bool IsPerforming { get; set; }

	public void AddCapability(CommandType command) {
		CommandCapabilities |= command;
	}
	
	public void RemoveCapability(CommandType command)
	{
		CommandCapabilities &= ~command;
	}
	
	public bool CanPerform(CommandType command)
	{
		return (CommandCapabilities & command) == command;
	}
}