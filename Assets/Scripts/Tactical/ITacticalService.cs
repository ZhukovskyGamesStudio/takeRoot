using System.Collections.Generic;
using UnityEngine;

public interface ITacticalService : IService {
	public void SetTacticalForSelectedSettlers();
	public void AddTacticalMovePosToSelectedSettlers(Vector3 pos);
}