using CodeBase.Services;
using UnityEngine;

namespace AI {
    public class ResourceCarrier : MonoBehaviour, IResourceCarrier {
        [SerializeField]
        private SpriteRenderer _resourceContainer;

        public void CarryResource(ResourceType type) {
            _resourceContainer.sprite = ServiceLocator.Container.Single<IResourceManager>().GetResourceSpriteNoShadow(type);
        }

        public void DropResource() {
            _resourceContainer.sprite = null;
        }
    }
}