using UnityEngine;

public interface ICommandParamsFactory : IService {
	public MoveCommandParams CreateMoveCommandParams();
	public DebugCommandParams CreateDebugCommandParams();
	public ICommandParams CreateDestroyCommandParams();
}