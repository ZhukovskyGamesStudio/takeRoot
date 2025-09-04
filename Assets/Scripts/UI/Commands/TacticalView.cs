using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Settlers.UI.Commands {
	public class TacticalView : MonoBehaviour {
		public GameObject TacticalContainer;
		
		public Button TacticalSwapButton;
		public TextMeshProUGUI TacticalDescription;
		
		public TextMeshProUGUI TacticalText;
		public Image TacticalIcon;
		public Image RegularIcon;
	}
}