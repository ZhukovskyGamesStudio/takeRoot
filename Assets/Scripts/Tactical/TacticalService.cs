using System.Collections.Generic;
using UnityEngine;

public class TacticalService : ITacticalService {
	private readonly IGridService _gridService;
	private List<AI.Settler> _selectedSettlers;

	public TacticalService(IGridService gridService) {
		_gridService = gridService;
		_selectedSettlers = new List<AI.Settler>();
	}
	
	public void SetSelected(List<AI.Settler> settlers) {
		_selectedSettlers = settlers;
	}

	public void SetTacticalForSelectedSettlers(bool isTactical) {
		foreach (var settler in _selectedSettlers) {
			settler.Data.tactical.IsTactical = isTactical;
		}
	}

	public void AddTacticalMovePosToSelectedSettlers(Vector3 pos) {
		Queue<Vector3> aroundPositions = GetAroundPos(pos);
		foreach (var settler in _selectedSettlers) {
			bool isFreePos = false;
			while (aroundPositions.Count > 0 || isFreePos == false) {
				var curPos = aroundPositions.Dequeue();
				if (!_gridService.IsOccupiedPos(curPos)) {
					isFreePos = true;
					settler.Data.tactical.TacticalMovePos = curPos;
					settler.Data.tactical.HasTacticalMovePos = true;
				}
			}
		}
	}
	private Queue<Vector3> GetAroundPos(Vector3 origin) {
		Queue<Vector3> aroundPositions = new Queue<Vector3>();
		aroundPositions.Enqueue(origin);			
		aroundPositions.Enqueue(origin + new Vector3(1, 0));
		aroundPositions.Enqueue(origin + new Vector3(1, -1));
		aroundPositions.Enqueue(origin + new Vector3(0, -1));
		aroundPositions.Enqueue(origin + new Vector3(-1, -1));
		aroundPositions.Enqueue(origin + new Vector3(-1, 0));
		aroundPositions.Enqueue(origin + new Vector3(-1, 1));
		aroundPositions.Enqueue(origin + new Vector3(0, 1));
		aroundPositions.Enqueue(origin + new Vector3(1, 1));
		return aroundPositions;
		
	}
}