using System;
using CodeBase.Services;
using TMPro;
using UnityEngine;

namespace GameResources {
    public class Resource : MonoBehaviour {
        [SerializeField]
        private ResourceType _resourceType;

        [SerializeField]
        private TextMeshPro _amountText;

        private IResourceManager _resourceManager;

        [field: SerializeField]
        public int Amount { get; private set; }

        public ResourceType Type => _resourceType;
        public int Reserved { get; set; }

        public void Init(int amount) {
            Amount = amount;
            _amountText.text = amount.ToString();
            _resourceManager = ServiceLocator.Container.Single<IResourceManager>();
        }

        public void PickUp(int amount) {
            Amount -= amount;
            Reserved -= amount;
            _amountText.text = Amount.ToString();
            if (Amount == 0) {
                _resourceManager.DestroyResource(transform.position);
            }
        }
    }

    [Serializable]
    public class ResourcesData {
        public ResourceType type;
        public int amount;
    }
}