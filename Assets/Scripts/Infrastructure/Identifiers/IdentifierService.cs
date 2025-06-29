public class IdentifierService : IIdentifierService {
	
	private int _lastId = 0;

	public int Next() => ++_lastId;
}