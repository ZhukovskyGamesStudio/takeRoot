using System;
using UniRx;
using UnityEditor.Experimental.GraphView;

namespace Settlers.UI.Commands {
	public class TacticalPresenter : IDisposable {
		private TacticalView _tacticalView;
		private readonly CommandView _commandView;
		private readonly ISelectionService _selection;
		private readonly ITacticalService _tacticalService;
		private readonly INetworkService _network;

		public TacticalPresenter(TacticalView tacticalView, CommandView commandView, ISelectionService selection, ITacticalService tacticalService, INetworkService network) {
			_tacticalView = tacticalView;
			_commandView = commandView;
			_selection = selection;
			_tacticalService = tacticalService;
			_network = network;
			_tacticalView.TacticalSwapButton.onClick.AddListener(() => {
				_tacticalService.SetTacticalForSelectedSettlers();
				UpdateView();
			});
			_selection.SelectedReactive.AsObservable().Subscribe(ShowTacticalButton);
		}

		private void ShowTacticalButton(Selectable selectable) {
			var button = _tacticalView.TacticalSwapButton;
			if (selectable == null) {
				button.gameObject.SetActive(false);
				return;
			}
			if (selectable is SettlerSelectable) {
				button.gameObject.SetActive(true);
			}
		}

		private void UpdateView() {
			var selectable = _selection.SelectedReactive.Value;
			if (selectable == null) {
				_commandView.transform.parent.gameObject.SetActive(true);
				return;
			}
			if (selectable is SettlerSelectable) {
				var data = (AI.SettlerData)selectable.GetData(_network.MyRace.Value);
				_commandView.transform.parent.gameObject.SetActive(!data.tactical.IsTactical);
			}
		}

		public void Dispose() {
			_tacticalView.TacticalSwapButton.onClick.RemoveAllListeners();
		}
	}
}