public interface ICommandService {

	public void HandleCommandRequest(CommandType type, bool withSelectedSettler);
	void CancelCommand(int id);
	void RegisterCommand(int id, BaseCommand baseCommand);
	void UnregisterCommand(int id);
}