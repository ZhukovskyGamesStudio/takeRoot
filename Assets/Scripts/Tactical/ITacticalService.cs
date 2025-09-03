using System.Collections.Generic;
using UnityEngine;

public interface ITacticalService : IService {
	public void SetSelected(List<AI.Settler> settlers);
	public void SetTacticalForSelectedSettlers(bool isTactical);
	public void AddTacticalMovePosToSelectedSettlers(Vector3 pos);
}