public static class CommandsExtensions {
	public static bool IsAvailableCommand(this CommandTarget target, CommandType command) {
		return (target.CommandCapabilities & command) == command;
	}

	public static bool CanPerform(this Worker performer, CommandType command) {
		return (performer.CommandCapabilities & command) == command;
	}
}