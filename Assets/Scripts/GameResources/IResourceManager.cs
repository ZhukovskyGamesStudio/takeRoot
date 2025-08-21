using UnityEngine;

namespace Settlers.Infrastructure {
	public interface IResourceManager : IService {
		public void SpawnResource(Vector3 at, ResourceType type, int amount);
	}
}