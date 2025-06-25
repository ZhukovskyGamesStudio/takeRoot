using System.Data;
using UnityEngine;

public interface ICommandService {
	public void HandleCommandRequest(CommandType command, Vector2 pos, ICommandTarget target);
}
