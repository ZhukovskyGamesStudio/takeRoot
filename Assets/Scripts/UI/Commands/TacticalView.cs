using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settlers.UI.Commands {
    public class TacticalView : MonoBehaviour {
        public GameObject TacticalContainer;

        public Toggle TacticalToggle;
        public TextMeshProUGUI TacticalDescription;

        public TextMeshProUGUI TacticalText;
        public Image TacticalIcon;
        public Image RegularIcon;

        private Action<bool> _onChange;

        public void Init(Action<bool> onChange) {
            _onChange = onChange;
        }

        public void Change(bool isOn) {
            _onChange?.Invoke(isOn);
        }
    }
}