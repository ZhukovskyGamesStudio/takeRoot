using System.Data;
using UnityEngine;

public interface ICommandService : IService{
	public void HandleCommandRequest(CommandType type);
	public void CancelCommand(int id);
}