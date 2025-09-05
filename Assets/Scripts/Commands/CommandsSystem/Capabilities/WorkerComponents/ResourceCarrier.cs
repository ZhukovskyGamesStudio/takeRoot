using CodeBase.Services;
using Unity.Netcode;
using UnityEngine;

namespace AI {
    public class ResourceCarrier : NetworkBehaviour, IResourceCarrier {
        [SerializeField]
        private SpriteRenderer _resourceContainer;

        public void CarryResource(ResourceType type) {
            SetSprite(type);
            CarryResourceClientRpc(type);
        }

        [ClientRpc]
        private void CarryResourceClientRpc(ResourceType type) {
            SetSprite(type);
        }

        private void SetSprite(ResourceType type) {
            _resourceContainer.sprite = ServiceLocator.Container.Single<IResourceManager>().GetResourceSpriteNoShadow(type);
        }

        public void DropResource() {
            _resourceContainer.sprite = null;
            DropResourceClientRpc();
        }

        [ClientRpc]
        private void DropResourceClientRpc() {
            _resourceContainer.sprite = null;
        }
    }
}