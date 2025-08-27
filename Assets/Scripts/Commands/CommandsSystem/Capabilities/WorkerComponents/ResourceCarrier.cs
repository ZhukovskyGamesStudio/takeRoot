using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class ResourceCarrier : MonoBehaviour, IResourceCarrier {
        [SerializeField]
        private List<Sprite> resources = new(5);

        [SerializeField]
        private SpriteRenderer _resourceContainer;

        public void CarryResource(ResourceType type) {
            switch (type) {
                case ResourceType.Planks:
                    _resourceContainer.sprite = resources[0];
                    break;
                case ResourceType.MetalScraps:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void DropResource() {
            _resourceContainer.sprite = null;
        }
    }
}