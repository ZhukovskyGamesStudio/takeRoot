using System.Data;
using UnityEngine;

public interface ICommandService : IService{
	public void HandleCommandRequest<TParams>(TParams commandParams) where TParams : ICommandParams;
}