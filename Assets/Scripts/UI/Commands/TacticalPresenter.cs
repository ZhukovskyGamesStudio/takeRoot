using System;
using UniRx;

namespace Settlers.UI.Commands {
	public class TacticalPresenter : IDisposable {
		private TacticalView _tacticalView;
		private readonly CommandView _commandView;
		private readonly ISelectionService _selection;
		private readonly ITacticalService _tacticalService;
		private readonly INetworkService _network;
		private string _tacticalText = "боевой режим\n (space)";
		private string _regularModeText = "в обычный режим\n (space)";

		public TacticalPresenter(TacticalView tacticalView, CommandView commandView, ISelectionService selection, ITacticalService tacticalService, INetworkService network) {
			_tacticalView = tacticalView;
			_commandView = commandView;
			_selection = selection;
			_tacticalService = tacticalService;
			_network = network;
			_tacticalView.Init(OnChange);
			_selection.SelectedReactive.AsObservable().Subscribe(_ => OnSelectionChange());

			_tacticalService.OnTacticalChanged += () => {
				_tacticalView.TacticalToggle.isOn = !_tacticalView.TacticalToggle.isOn;
			};
		}

		private void OnChange(bool isOn) {
			_tacticalService.SetTacticalForSelectedSettlers(isOn);
			OnSelectionChange();
			SetTacticalElements(isOn);
		}

		private void OnSelectionChange() {
			var selectable = _selection.SelectedReactive.Value;
			if (selectable == null) {
				_commandView.ToggleContainer.SetActive(true);
				_tacticalView.TacticalContainer.SetActive(false);
				return;
			}
			if (selectable is SettlerSelectable) {
				var data = selectable.GetData(_network.MyRace.Value);
				if (data is AI.SettlerData settlerData) {
					var isTactical = settlerData.tactical.IsTactical;
					_tacticalView.TacticalContainer.SetActive(true);
					_commandView.ToggleContainer.SetActive(!isTactical);
					SetTacticalElements(isTactical);
				} else {
					_commandView.ToggleContainer.SetActive(true);
					_tacticalView.TacticalContainer.SetActive(false);
				}
			}
		}

		private void SetTacticalElements(bool isTactical) {
			_tacticalView.TacticalToggle.SetIsOnWithoutNotify(isTactical);
			_tacticalView.TacticalDescription.gameObject.SetActive(isTactical);
			_tacticalView.TacticalIcon.gameObject.SetActive(!isTactical);
			_tacticalView.RegularIcon.gameObject.SetActive(isTactical);
			_tacticalView.TacticalText.text = isTactical ? _regularModeText : _tacticalText;
		}
		public void Dispose() {
			
		}
	}
}