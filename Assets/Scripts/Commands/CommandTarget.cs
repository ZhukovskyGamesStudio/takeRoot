using System;
using UnityEngine;

public class CommandTarget : MonoBehaviour {
	public int ObjectId { get;}
	public int CommandId { get; set; }
	
	public Action onDestroy;

	public bool CanBeCommanded {get; set;} = true;
	public CommandType CommandCapabilities { get; set; }

	private void Awake() {
		AddCapability(CommandType.Destroy);
	}

	public void AddCapability(CommandType command) {
		CommandCapabilities |= command;
	}
	
	public void RemoveCapability(CommandType command)
	{
		CommandCapabilities &= ~command;
	}

	private void OnDestroy() {
		onDestroy?.Invoke();
	}
}