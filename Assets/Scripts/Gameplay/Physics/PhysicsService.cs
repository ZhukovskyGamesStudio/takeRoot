using UnityEngine;

public class PhysicsService : IPhysicsService
{
	private readonly RaycastHit2D[] _hits = new RaycastHit2D[10];
	
	public T Raycast<T>(Vector2 worldPosition, Vector2 direction) where T : class {
		RaycastHit2D hit = Physics2D.Raycast(worldPosition, direction);
		
		if (hit.collider == null)
			return null;
			
		T obj = hit.collider.GetComponent<T>();
		return obj;
	}
}