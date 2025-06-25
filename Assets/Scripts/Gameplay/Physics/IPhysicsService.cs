using UnityEngine;

public interface IPhysicsService : IService
{
	public T Raycast<T>(Vector2 worldPosition, Vector2 direction) where T : class;
}