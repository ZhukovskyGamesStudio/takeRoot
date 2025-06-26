using UnityEngine;

public interface IGameFactory : IService
{
    GameObject CreateSettler(string settlersTypeId, Vector3 at);

    GameObject CreateResource(string resourceId, Vector3 at, int amount);
    ICommand CreateCommand<TParams>(TParams commandParams) where TParams : ICommandParams;

}