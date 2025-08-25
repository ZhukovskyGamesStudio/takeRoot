using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources {
	public class Resource : MonoBehaviour {
		[SerializeField]private ResourceType _resourceType;
		[SerializeField]private TextMeshPro _amountText;
		[field: SerializeField]
		public int Amount { get; private set; }
		public ResourceType Type => _resourceType;
		public int Reserved { get; set; }
		public void Init(int amount) {
			Amount = amount;
			_amountText.text = amount.ToString();
		}

		public void PickUp(int amount) {
			Amount -= amount;
			Reserved -= amount;
			_amountText.text = Amount.ToString();
			if (Amount == 0) {
				Destroy(this.gameObject);
			}
		}
	}
	[Serializable]
	public class ResourcesData {
		public ResourceType type;
		public int amount;
	}
}